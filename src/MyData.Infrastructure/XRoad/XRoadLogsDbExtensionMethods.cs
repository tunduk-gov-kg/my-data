using MyData.Core.Models;
using Npgsql;

namespace MyData.Infrastructure.XRoad
{
    public static class XRoadLogsDbExtensionMethods
    {
        public static string BuildConnectionString(this XRoadLogsDb sourceDb)
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder
            {
                Host = sourceDb.Host,
                Port = sourceDb.Port,
                Username = sourceDb.Username,
                Password = sourceDb.Password,
                Database = sourceDb.Database,
            };
            return connectionStringBuilder.ConnectionString;
        }
    }
}