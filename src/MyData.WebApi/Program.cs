using Coravel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyData.WebApi.BackgroundService;

namespace MyData.WebApi
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.Services.UseScheduler(scheduler =>
            {
                scheduler.Schedule<LogsCollectorTask>().Daily();
                scheduler.Schedule<RequestsCleanerTask>().DailyAtHour(6);
            });

            host.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddScheduler();
                    services.AddTransient<LogsCollectorTask>();
                    services.AddTransient<RequestsCleanerTask>();
                    services.AddSingleton<IStartupFilter, Migration>();
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}