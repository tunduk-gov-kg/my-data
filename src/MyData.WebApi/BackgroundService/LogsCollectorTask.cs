using System;
using System.Threading.Tasks;
using Coravel.Invocable;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyData.Core.Interfaces;

namespace MyData.WebApi.BackgroundService
{
    public class LogsCollectorTask : IInvocable
    {
        private readonly IXRoadLogsDbListProvider _xRoadDbListProvider;

        private readonly IServiceProvider _serviceProvider;

        private readonly ILogger<LogsCollectorTask> _logger;

        public LogsCollectorTask(IXRoadLogsDbListProvider xRoadDbListProvider, IServiceProvider serviceProvider, ILogger<LogsCollectorTask> logger)
        {
            _xRoadDbListProvider = xRoadDbListProvider;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task Invoke()
        {
            _logger.LogInformation("Invoking LogsCollectorTask");

            Parallel.ForEach(_xRoadDbListProvider.List, async targetDb =>
            {
                _logger.LogInformation("Processing db: host - {0}, port - {1}, dbname - {2}", targetDb.Host,targetDb.Port,targetDb.Database);
                using var serviceScope = _serviceProvider.CreateScope();
                var collectorService = serviceScope.ServiceProvider.GetRequiredService<LogsCollectorService>();
                await collectorService.Collect(targetDb);
            });
            
            _logger.LogInformation("LogsCollectorTask completed");
        }
    }
}