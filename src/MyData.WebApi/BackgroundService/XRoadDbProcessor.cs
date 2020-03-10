using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyData.Core.Interfaces;

namespace MyData.WebApi.BackgroundService
{
    public class XRoadDbProcessor : IHostedService, IDisposable
    {
        private readonly ILogger<XRoadDbProcessor> _logger;
        private readonly IXRoadDbReader _dbReader;
        private Timer _timer;

        public XRoadDbProcessor(ILogger<XRoadDbProcessor> logger, IXRoadDbReader dbReader)
        {
            _logger = logger;
            _dbReader = dbReader;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(6));
            return Task.CompletedTask;
        }


        private void DoWork(object state)
        {
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }


        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}