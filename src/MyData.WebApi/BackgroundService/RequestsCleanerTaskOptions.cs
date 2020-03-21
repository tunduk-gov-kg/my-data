namespace MyData.WebApi.BackgroundService
{
    public class RequestsCleanerTaskOptions
    {
        /// <summary>
        /// Days count
        /// </summary>
        public long RequestsLifetime { get; set; } = 30;
    }
}