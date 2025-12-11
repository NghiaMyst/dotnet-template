
using dotnet_boilderplate.DummyService.Persistence;
using dotnet_boilderplate.SharedKernel.Messaging;
using Microsoft.EntityFrameworkCore;

namespace dotnet_boilderplate.DummyService.BackgroundServices
{
    public class HandleOutboxMessageService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly ILogger<HandleOutboxMessageService> _logger;

        public HandleOutboxMessageService(IServiceProvider serviceProvider, ILogger<HandleOutboxMessageService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<DummyDbContext>();
                var publisher = scope.ServiceProvider.GetRequiredService<IDomainEventPublisher>();

                var unhandledMessages = await dbContext.OutboxMessages
                    .Where(m => m.Published == false)
                    .OrderBy(m => m.CreatedAt)
                    .Take(100)
                    .ToListAsync(stoppingToken);

                if (unhandledMessages.Count <= 0) return;

                // Published through publisher

                foreach (var message in unhandledMessages)
                {
                    try
                    {
                        await publisher.PublishAsync(message.Type, message.Payload, stoppingToken);
                        message.MarkAsPublished();
                        _logger.LogInformation("HandleOutboxMessageService: Successfully published {message}", message.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("HandleOutboxMessageService: {message}", ex.Message);
                    }

                    if (unhandledMessages.Any(m => m.Published))
                    {
                        await dbContext.SaveChangesAsync(stoppingToken);
                    }

                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }

            }
        }
    }
}
