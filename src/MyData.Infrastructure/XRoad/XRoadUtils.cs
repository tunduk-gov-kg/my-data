using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using MyData.Core.Models;

namespace MyData.Infrastructure.XRoad
{
    public static class XRoadUtils
    {
        private static readonly string KgzPinPattern = "[012]{1}[0,1,2,3]{1}[0-9]{1}[0,1]{1}[0-9]{1}[1,2]{1}[0-9]{3}[0-9]{5}";
        
        public static XRoadRequest ParseSoap(XRoadLog xRoadLog)
        {
            var document = XDocument.Parse(xRoadLog.Message);

            var service = document
                .Descendants()
                .First(tag => tag.Name.LocalName.Equals("service") &&
                              tag.Name.NamespaceName.Equals("http://x-road.eu/xsd/xroad.xsd"));

            var client = document.Descendants()
                .First(tag => tag.Name.LocalName.Equals("client") &&
                              tag.Name.NamespaceName.Equals("http://x-road.eu/xsd/xroad.xsd"));

            var messageId = document.Descendants()
                .First(tag => tag.Name.LocalName.Equals("id") &&
                              tag.Name.NamespaceName.Equals("http://x-road.eu/xsd/xroad.xsd")).Value;

            var messageIssue = document.Descendants()
                .First(tag => tag.Name.LocalName.Equals("issue") &&
                              tag.Name.NamespaceName.Equals("http://x-road.eu/xsd/xroad.xsd")).Value;

            var userId = document.Descendants()
                .First(tag => tag.Name.LocalName.Equals("userId") &&
                              tag.Name.NamespaceName.Equals("http://x-road.eu/xsd/xroad.xsd")).Value;

            var regex = new Regex(KgzPinPattern, RegexOptions.None);
            
            var match = regex.Match(xRoadLog.Message);

            return new XRoadRequest
            {
                ServiceInvokedAt = DateTimeOffset.FromUnixTimeSeconds(xRoadLog.Time).UtcDateTime,

                ClientXRoadInstance =
                    client.Descendants().First(tag => tag.Name.LocalName.Equals("xRoadInstance")).Value,
                ClientMemberClass = client.Descendants().First(tag => tag.Name.LocalName.Equals("memberClass")).Value,
                ClientMemberCode = client.Descendants().First(tag => tag.Name.LocalName.Equals("memberCode")).Value,
                ClientSubsystemCode = client.Descendants()
                    .FirstOrDefault(tag => tag.Name.LocalName.Equals("subsystemCode"))?.Value,

                ServiceXRoadInstance =
                    service.Descendants().First(tag => tag.Name.LocalName.Equals("xRoadInstance")).Value,
                ServiceMemberClass = service.Descendants().First(tag => tag.Name.LocalName.Equals("memberClass")).Value,
                ServiceMemberCode = service.Descendants().First(tag => tag.Name.LocalName.Equals("memberCode")).Value,
                ServiceSubsystemCode = service.Descendants()
                    .FirstOrDefault(tag => tag.Name.LocalName.Equals("subsystemCode"))?.Value,
                ServiceCode = service.Descendants().First(tag => tag.Name.LocalName.Equals("serviceCode")).Value,
                ServiceVersion = service.Descendants()
                    .FirstOrDefault(tag => tag.Name.LocalName.Equals("serviceVersion"))?.Value,

                UserId = userId,
                MessageId = messageId,
                MessageIssue = messageIssue,

                XRequestId = xRoadLog.XRequestId,
                Pin = match.Success ? match.Value : null
            };
        }

        public static XRoadRequest ParseRest(XRoadLog xRoadLog)
        {
            var serviceCodePattern =
                @"\/r1\/[a-zA-Z0-9-]+\/[a-zA-Z0-9-]+\/[0-9]*\/[a-zA-Z0-9-_]+\/[a-zA-Z0-9-_]+\/[a-zA-Z0-9-_]*\/*";

            throw new NotImplementedException();
        }

        public static XRoadService DetectSoapService(XRoadLog xRoadLog)
        {
            throw new NotImplementedException();
        }

        public static XRoadService DetectRestService(XRoadLog xRoadLog)
        {
            throw new NotImplementedException();
        }
    }
}