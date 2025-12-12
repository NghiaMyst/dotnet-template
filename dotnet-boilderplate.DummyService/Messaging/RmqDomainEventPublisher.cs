using dotnet_boilderplate.SharedKernel.Messaging;
using RabbitMQ.Client;
using System.Text;

namespace dotnet_boilderplate.DummyService.Messaging;

public sealed class RmqDomainEventPublisher : IDomainEventPublisher, IAsyncDisposable
{
    private readonly IConnection _connection;
    private IChannel? _channel;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private ILogger<RmqDomainEventPublisher> _logger;
    private readonly string _exchangeName;
    private bool _disposed;

    public RmqDomainEventPublisher(IConnection connection, ILogger<RmqDomainEventPublisher> logger, string exchangeName="domain_events")
    {
        _connection = connection;
        _logger = logger;
        _exchangeName = exchangeName;
    }

    public async Task PublishAsync(string routingKeyOrTopic, string payload, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentException.ThrowIfNullOrEmpty(routingKeyOrTopic);
        ArgumentException.ThrowIfNullOrEmpty(payload);

        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            if (_channel == null)
            {
                _channel = await _connection.CreateChannelAsync(null, cancellationToken);

                await _channel.ExchangeDeclareAsync(
                        exchange: _exchangeName,
                        type: ExchangeType.Topic,
                        durable: true,
                        autoDelete: false,
                        arguments: null,
                        false,
                        cancellationToken
                    );
            }

            var body = Encoding.UTF8.GetBytes( payload );

            var properties = new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json",
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            };

            await _channel.BasicPublishAsync(
                exchange: _exchangeName,
                routingKey: routingKeyOrTopic,
                mandatory: false,
                basicProperties: properties,
                body: body,
                cancellationToken: cancellationToken
            );
        }
        catch (Exception ex)
        {
            _logger.LogError("RabbitMqProducer: {}", ex.Message);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;

        await _semaphore.WaitAsync();

        try
        {
            if (_channel != null)
            {
                await _channel.CloseAsync(cancellationToken: default);
                await _channel.DisposeAsync();
            }

            // Close connection
            // Connection dispose will be handled by app
            //await _connection.CloseAsync(cancellationToken: default);
            //await _connection.DisposeAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error disposing RabbitMQ resources: {ex.Message}");
        }
        finally
        {
            _semaphore.Dispose();
            _disposed = true;
        }
    }
}