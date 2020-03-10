namespace MyData.Core.Models
{
    /// <summary>
    /// Database where x-road requests are stored
    /// </summary>
    public class XRoadLogsDb
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Database { get; set; } = "messagelog";
    }
}