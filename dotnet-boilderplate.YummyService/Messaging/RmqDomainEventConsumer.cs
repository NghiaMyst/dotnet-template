
using dotnet_boilderplate.YummyService.Domains.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace dotnet_boilderplate.YummyService.Messaging
{
    public class RmqDomainEventConsumer : BackgroundService
    {
        private ILogger<RmqDomainEventConsumer> _logger;
        private readonly IConnection _connection;
        private IChannel? _channel;

        public RmqDomainEventConsumer(ILogger<RmqDomainEventConsumer> logger, IConnection connection)
        {
            _logger = logger;
            _connection = connection;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await _channel.ExchangeDeclareAsync(
                    "domain_events",
                    ExchangeType.Topic,
                    durable: true,
                    autoDelete: false,
                    null,
                    false,
                    false,
                    stoppingToken
                );

            // currently fix
            // canbe modified to more dynamic when implement contracts
            var queueName = "notification_service_queue";
            await _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
            await _channel.QueueBindAsync(queue: queueName, exchange: "domain_events", routingKey: "OrderCreatedDomainEvent", cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;

                try
                {
                    _logger.LogInformation("Received event: {RoutingKey} | Payload: {Payload}", routingKey, message);

                    if (routingKey == "OrderCreatedEvent")
                    {
                        var orderEvent = JsonConvert.DeserializeObject<OrderCreatedDomainEvent>(message);

                        _logger.LogInformation("Order {OrderId} created → Sending notification to customer {CustomerId}", orderEvent?.OrderId, orderEvent?.CustomerId);
                    }

                    await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message from {RoutingKey}", routingKey);
                    await _channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true, cancellationToken: stoppingToken);
                }
            };


            await _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel != null)
                await _channel.CloseAsync(200, "Service stopping");
            await base.StopAsync(cancellationToken);
        }
    }
}
