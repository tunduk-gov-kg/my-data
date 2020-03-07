namespace MyData.Core.Models
{
    /// <summary>
    /// Database where x-road requests are stored
    /// </summary>
    public class RequestsProviderDb
    {
        public string Host { get; set; }

        public string Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}