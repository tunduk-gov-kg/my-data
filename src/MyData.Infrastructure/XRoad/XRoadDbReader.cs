using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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

        private readonly IList<XRoadService> _restServices;

        private readonly IList<XRoadService> _soapServices;

        private readonly IMapper _mapper;

        public XRoadDbReader(ILogger<XRoadDbReader> logger, IList<XRoadService> targetServices, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _restServices = targetServices.Where(it => it.IsRestService).ToList();
            _soapServices = targetServices.Where(it => !it.IsRestService).ToList();
        }

        public List<XRoadRequest> Read(XRoadLogsDb sourceDb, long fromIdInclusive, long toIdInclusive)
        {
            _logger.LogInformation("Requesting x-road logs from db: {0}, ids: [{1},{2}]", sourceDb.Host,
                fromIdInclusive, toIdInclusive);

            var result = new List<XRoadRequest>();

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

                if (xRoadLog.Response || xRoadLog.Discriminator != "m") continue;

                if (TryToParse(xRoadLog, out var request))
                {
                    result.Add(request);
                }
            }

            return result;
        }

        private bool TryToParse(XRoadLog xRoadLog, out XRoadRequest request)
        {
            try
            {
                // ReSharper disable once ConvertIfStatementToReturnStatement
                if (xRoadLog.Message.StartsWith('<')) //soap message body start
                {
                    return TryToParseSoapMessage(xRoadLog, out request);
                }

                return TryToParseRestMessage(xRoadLog, out request);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, nameof(TryToParse));
                request = null;
                return false;
            }
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local
        private bool TryToParseRestMessage(XRoadLog xRoadLog, out XRoadRequest request)
        {
            var restPathMatch = XRoadRestMessageUtils.MatchServiceRestPath(xRoadLog.Message);
            if (restPathMatch.Success)
            {
                var xRoadService = XRoadRestMessageUtils.FindMatchingRestPath(_restServices, restPathMatch.Value);
                if (xRoadService == null)
                {
                    request = null;
                    return false;
                }

                //search pin in service path/query parameters
                var targetPin = XRoadRestMessageUtils.ParsePin(restPathMatch.Value);
                if (targetPin != null)
                {
                    //TODO: implemented pin search in rest message body
                    // rest message body is stored in postgres large objects table
                    // xroadlog references them by attachment oid                    
                    request = new XRoadRequest();
                    request.SetXRoadService(xRoadService);
                    request.SetXRoadClient(XRoadRestMessageUtils.ParseXRoadClient(xRoadLog.Message));
                    request.Pin = targetPin;
                    request.XRequestId = xRoadLog.XRequestId;
                    request.UserId = XRoadRestMessageUtils.ParseXRoadUserId(xRoadLog.Message);
                    request.MessageId = XRoadRestMessageUtils.ParseXRoadMessageId(xRoadLog.Message);
                    request.MessageIssue = XRoadRestMessageUtils.ParseXRoadMessageIssue(xRoadLog.Message);
                    request.ServiceInvokedAt = DateTimeOffset.FromUnixTimeSeconds(xRoadLog.Time).UtcDateTime;
                    return true;
                }
            }

            request = null;
            return false;
        }

        private bool TryToParseSoapMessage(XRoadLog xRoadLog, out XRoadRequest request)
        {
            var soapMessage = XDocument.Parse(xRoadLog.Message);

            var xRoadService = XRoadSoapMessageUtils.ParseXRoadService(soapMessage);

            if (_soapServices.Any(targetService => targetService.SameAs(xRoadService)))
            {
                var targetPin = XRoadSoapMessageUtils.ParsePin(soapMessage);
                if (targetPin != null)
                {
                    request = new XRoadRequest();
                    request.SetXRoadService(xRoadService);
                    request.SetXRoadClient(XRoadSoapMessageUtils.ParseXRoadClient(soapMessage));
                    request.Pin = targetPin;
                    request.XRequestId = xRoadLog.XRequestId;
                    request.UserId = XRoadSoapMessageUtils.ParseXRoadUserId(soapMessage);
                    request.MessageId = XRoadSoapMessageUtils.ParseXRoadMessageId(soapMessage);
                    request.MessageIssue = XRoadSoapMessageUtils.ParseXRoadMessageIssue(soapMessage);
                    request.ServiceInvokedAt = DateTimeOffset.FromUnixTimeSeconds(xRoadLog.Time).UtcDateTime;
                    return true;
                }
            }

            request = null;
            return false;
        }
    }
}