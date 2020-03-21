namespace MyData.WebApi.BackgroundService
{
    public class LogsCollectorOptions
    {
        public int CollectorIterationsCount { get; set; } = 100;

        public int Limit { get; set; } = 10_000;
    }
}