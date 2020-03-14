using AutoMapper;
using Microsoft.Extensions.Logging;
using MyData.Core.Interfaces;
using MyData.Core.Models;
using Npgsql;
using NpgsqlTypes;

namespace MyData.Infrastructure.XRoad
{
    public class XRoadDbReader : IXRoadDbReader
    {
        private readonly ILogger<XRoadDbReader> _logger;

        private readonly IMapper _mapper;

        public XRoadDbReader(ILogger<XRoadDbReader> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public XRoadLogsReadResult Read(XRoadLogsDb sourceDb, long fromIdInclusive, long toIdInclusive)
        {
            _logger.LogInformation("Requesting x-road logs from db: {0}, ids: [{1},{2}]", sourceDb.Host,
                fromIdInclusive, toIdInclusive);

            var result = new XRoadLogsReadResult(fromIdInclusive, toIdInclusive);

            var connectionString = sourceDb.BuildConnectionString();
            using var connection = new NpgsqlConnection(connectionString);

            var command = new NpgsqlCommand(
                "select id,queryid,memberclass,membercode,subsystemcode,message,time,attachment,xrequestid,response,discriminator from logrecord "
                + "where id >= @from_id_inclusive and id <= @to_id_inclusive "
                + "order by id", connection
            );

            command.Parameters.AddWithValue("from_id_inclusive", NpgsqlDbType.Bigint, fromIdInclusive);
            command.Parameters.AddWithValue("to_id_inclusive", NpgsqlDbType.Bigint, toIdInclusive);

            connection.Open();

            using var reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                return result;
            }

            while (reader.Read())
            {
                var xRoadLog = _mapper.Map<XRoadLog>(reader);
                result.XRoadLogs.Add(xRoadLog);
            }

            return result;
        }
    }
}