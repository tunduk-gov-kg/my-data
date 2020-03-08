using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Logging;
using MyData.Core.ExtensionMethods;
using MyData.Core.Interfaces;
using MyData.Core.Models;
using Npgsql;
using NpgsqlTypes;

namespace MyData.Infrastructure.XRoad
{
    public class XRoadDbReader : IXRoadDbReader
    {
        private readonly ILogger<XRoadDbReader> _logger;

        private readonly IList<XRoadService> _targetServices;

        public XRoadDbReader(ILogger<XRoadDbReader> logger, IList<XRoadService> targetServices)
        {
            _logger = logger;
            _targetServices = targetServices;
        }

        public List<XRoadRequest> Read(XRoadLogsDb sourceDb, long fromIdInclusive, long toIdInclusive)
        {
            _logger.LogInformation("Requesting x-road logs from db: {0}, ids: [{1},{2}]", sourceDb.Host,
                fromIdInclusive, toIdInclusive);

            var result = new List<XRoadRequest>();

            var connectionString = sourceDb.BuildConnectionString();
            using var connection = new NpgsqlConnection(connectionString);

            var command = new NpgsqlCommand(
                "select id,queryid,memberclass,membercode,subsystemcode,message,time,attachment,xrequestid,response from logrecord "
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
                var xRoadLog = Map(reader);
                if (xRoadLog.Response) continue;
                if (TryToParse(xRoadLog, out var request))
                {
                    result.Add(request);
                }
            }

            return result;
        }

        private static XRoadLog Map(NpgsqlDataReader reader)
        {
            return new XRoadLog
            {
                Id = reader.GetInt64("id"),
                QueryId = !reader.IsDBNull("queryid") ? reader.GetString("queryid") : null,
                MemberClass = reader.GetString("memberclass"),
                MemberCode = reader.GetString("membercode"),
                SubSystemCode = !reader.IsDBNull("subsystemcode") ? reader.GetString("subsystemcode") : null,
                Message = reader.GetString("message"),
                Time = reader.GetInt64("time"),
                Attachment = !reader.IsDBNull("attachment") ? reader.GetInt64("attachment") : (long?) null,
                XRequestId = !reader.IsDBNull("xrequestid") ? reader.GetString("xrequestid") : null,
                Response = reader.GetBoolean("response")
            };
        }

        private bool TryToParse(XRoadLog xRoadLog, out XRoadRequest request)
        {
            if (xRoadLog.Message.StartsWith('<')) //soap message body start
            {
                var soapService = XRoadUtils.DetectSoapService(xRoadLog);

                if (_targetServices.Any(targetService => targetService.SameAs(soapService)))
                {
                    request = XRoadUtils.ParseSoap(xRoadLog);
                    return true;
                }

                request = null;
                return false;
            }

            var restService = XRoadUtils.DetectRestService(xRoadLog);
            if (_targetServices.Any(targetService => targetService.SameAs(restService)))
            {
                request = XRoadUtils.ParseSoap(xRoadLog);
                return true;
            }

            request = null;
            return false;
        }
    }
}