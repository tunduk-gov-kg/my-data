using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using MyData.Core.Interfaces;
using MyData.Core.Models;
using Nito.AsyncEx.Synchronous;

namespace MyData.Infrastructure.XRoad
{
    public class XRoadLogsProcessor : IXRoadLogsProcessor
    {
        private readonly IXRoadServiceStore _serviceStore;
        
        private List<XRoadService> _restServices;

        private List<XRoadService> _soapServices;

        private readonly ILogger<XRoadLogsProcessor> _logger;

        // ReSharper disable once InvertIf
        private List<XRoadService> RestServices
        {
            get
            {
                return _restServices ??= _serviceStore.GetRestServicesAsync().WaitAndUnwrapException();
            }  
        } 

        private List<XRoadService> SoapServices
        {
            get
            {
                // ReSharper disable once InvertIf
                return _soapServices ??= _serviceStore.GetSoapServicesAsync().WaitAndUnwrapException();
            }
        }

        public XRoadLogsProcessor(IXRoadServiceStore serviceStore, ILogger<XRoadLogsProcessor> logger)
        {
            _serviceStore = serviceStore;
            _logger = logger;
        }

        public List<XRoadRequest> Process(List<XRoadLog> logs)
        {
            var result = new List<XRoadRequest>(logs.Capacity);

            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var xRoadLog in logs)
            {
                if (!xRoadLog.Response.HasValue) //require response field is not null
                {
                    continue;
                }

                if ((bool) xRoadLog.Response) //require response is not true
                {
                    continue;
                }

                if (xRoadLog.Discriminator != "m") //get only messages logs
                {
                    continue;
                }
                
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
                var xRoadService = XRoadRestMessageUtils.FindMatchingRestPath(RestServices, restPathMatch.Value);
                if (xRoadService == null)
                {
                    request = null;
                    return false;
                }

                //search pin in service path/query parameters
                var targetPin = XRoadRestMessageUtils.ParsePin(restPathMatch.Value, xRoadService.ParseRule);
                
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
                    request.ServiceInvokedAt = DateTimeOffset.FromUnixTimeMilliseconds(xRoadLog.Time).UtcDateTime;
                    
                    return true;
                }
            }

            request = null;
            return false;
        }

        private bool TryToParseSoapMessage(XRoadLog xRoadLog, out XRoadRequest request)
        {
            var soapMessage = XDocument.Parse(xRoadLog.Message);

            var incomingSoapService = XRoadSoapMessageUtils.ParseXRoadService(soapMessage);
            
            var soapServiceFromDb = SoapServices.FirstOrDefault(targetService => targetService.SameAs(incomingSoapService));
            
            if (soapServiceFromDb != null)
            {
                var targetPin = XRoadSoapMessageUtils.ParsePin(soapMessage, soapServiceFromDb.ParseRule);
                
                if (targetPin != null)
                {
                    request = new XRoadRequest();
                    request.SetXRoadService(incomingSoapService);
                    request.SetXRoadClient(XRoadSoapMessageUtils.ParseXRoadClient(soapMessage));
                    request.Pin = targetPin;
                    request.XRequestId = xRoadLog.XRequestId;
                    request.UserId = XRoadSoapMessageUtils.ParseXRoadUserId(soapMessage);
                    request.MessageId = XRoadSoapMessageUtils.ParseXRoadMessageId(soapMessage);
                    request.MessageIssue = XRoadSoapMessageUtils.ParseXRoadMessageIssue(soapMessage);
                    request.ServiceInvokedAt = DateTimeOffset.FromUnixTimeMilliseconds(xRoadLog.Time).UtcDateTime;
                    return true;
                }
            }

            request = null;
            return false;
        }
    }
}