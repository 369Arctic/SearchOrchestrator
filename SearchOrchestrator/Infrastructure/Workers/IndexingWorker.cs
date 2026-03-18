using SearchOrchestrator.Application.Interfaces;
using System.ComponentModel;

namespace SearchOrchestrator.Infrastructure.Workers
{
    public class IndexingWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<IndexingWorker> _logger;

        public IndexingWorker(IServiceProvider serviceProvider, ILogger<IndexingWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var service = scope.ServiceProvider.GetRequiredService<IIndexingService>();

                    await service.ProcessAsync(ct);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Worker iteration error");
                }

                // Упрощенный polling в рамках тестового. В проде необходим делать через очередь. 
                await Task.Delay(2000, ct);
            }
        }
    }
}
