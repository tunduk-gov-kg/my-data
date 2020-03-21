using System;
using System.Threading.Tasks;
using Coravel.Invocable;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyData.Core.Interfaces;

namespace MyData.WebApi.BackgroundService
{
    public class RequestsCleanerTask : IInvocable
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly ILogger<RequestsCleanerTask> _logger;

        private readonly RequestsCleanerTaskOptions _options;

        public RequestsCleanerTask(IServiceProvider serviceProvider, ILogger<RequestsCleanerTask> logger,
            IOptions<RequestsCleanerTaskOptions> options)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _options = options.Value;
        }

        public async Task Invoke()
        {
            _logger.LogInformation("Invoking RequestsCleanerTask");

            using var scope = _serviceProvider.CreateScope();

            var requestStore = scope.ServiceProvider.GetRequiredService<IXRoadRequestStore>();

            //substract days from current date time
            var deleteUntil = DateTime.Now.AddDays(-_options.RequestsLifetime);

            var deletedCount = await requestStore.PurgeAsync(deleteUntil);

            _logger.LogInformation("RequestsCleanerTask completed, {0} deleted", deletedCount);
        }
    }
}