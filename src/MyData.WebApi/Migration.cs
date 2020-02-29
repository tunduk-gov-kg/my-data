using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace MyData.WebApi
{
    public class Migration : IStartupFilter
    {
        private readonly IConfiguration _configuration;

        private readonly ILogger<Migration> _logger;

        public Migration(IConfiguration configuration, ILogger<Migration> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            try
            {
                var cnx = new NpgsqlConnection(_configuration.GetConnectionString("MyDataDb"));
                var evolve = new Evolve.Evolve(cnx, message => _logger.LogInformation(message))
                {
                    Locations = new[] {"Migration/Postgres"},
                    IsEraseDisabled = true,
                };

                evolve.Migrate();
            }
            catch (Exception exception)
            {
                _logger.LogCritical("Database migration failed.", exception);
                throw;
            }
            
            return next;
        }
    }
}