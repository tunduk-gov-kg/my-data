using AutoMapper;
using Microsoft.Extensions.Logging;
using MyData.Core.Interfaces;
using MyData.Core.Models;
using Npgsql;
using NpgsqlTypes;
// ReSharper disable StringLiteralTypo

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

        public XRoadLogsReadResult Read(XRoadLogsDb sourceDb, long fromIdInclusive, int limit)
        {
            _logger.LogInformation("Requesting x-road logs from db: {0}, ids: [{1},{2}]", sourceDb.Host,
                fromIdInclusive, limit);

            var result = new XRoadLogsReadResult();

            var connectionString = sourceDb.BuildConnectionString();
            using var connection = new NpgsqlConnection(connectionString);

            var command = new NpgsqlCommand(
                "select id,queryid,memberclass,membercode,subsystemcode,message,time,attachment,xrequestid,response,discriminator from logrecord "
                + "where id >= @from_id_inclusive "
                + "order by id "
                + "limit @limit", connection
            );

            command.Parameters.AddWithValue("from_id_inclusive", NpgsqlDbType.Bigint, fromIdInclusive);
            command.Parameters.AddWithValue("limit", NpgsqlDbType.Integer, limit);
            
            connection.Open();

            using var reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                return result;
            }

            while (reader.Read())
            {
                var xRoadLog = _mapper.Map<XRoadLog>(reader);
                result.Add(xRoadLog);
            }

            return result;
        }

        public bool AnyRecords(XRoadLogsDb sourceDb, long fromIdInclusive)
        {
            var connectionString = sourceDb.BuildConnectionString();
            using var connection = new NpgsqlConnection(connectionString);
            var command = new NpgsqlCommand("select count(*) from logrecord where id >= @from_id_inclusive", connection);
            command.Parameters.AddWithValue("from_id_inclusive", NpgsqlDbType.Bigint, fromIdInclusive);
            connection.Open();
            var result = (long) command.ExecuteScalar();
            connection.Close();
            return result > 0;
        }
    }
}