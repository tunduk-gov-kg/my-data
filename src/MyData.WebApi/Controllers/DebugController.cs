using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyData.Core.Interfaces;
using MyData.Core.Models;

namespace MyData.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DebugController : ControllerBase
    {
        private readonly IXRoadDbReader _dbReader;
        private readonly ILogger<DebugController> _logger;

        public DebugController(IXRoadDbReader dbReader, ILogger<DebugController> logger)
        {
            _dbReader = dbReader;
            _logger = logger;
        }

        public List<XRoadRequest> Execute()
        {
            var requests = _dbReader.Read(new XRoadLogsDb
            {
                Host = "localhost",
                Port = 5432,
                Username = "postgres",
                Password = "postgres",
                Database = "XRoadDb"
            }, 0, 200_000);

            return requests;
        }
    }
}