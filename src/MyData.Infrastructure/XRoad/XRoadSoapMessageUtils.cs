using System.Linq;
using System.Xml.Linq;
using MyData.Core;
using MyData.Core.Models;

namespace MyData.Infrastructure.XRoad
{
    public static class XRoadSoapMessageUtils
    {
        public static string ParsePin(XDocument soapMessage)
        {
            var soapMessageBody = soapMessage.Descendants()
                .First(tag =>
                    tag.Name.LocalName.Equals("Body") &&
                    tag.Name.NamespaceName.Equals("http://schemas.xmlsoap.org/soap/envelope/"))
                .ToString();

            var match = MyDataConstants.RegEx.KgzPinRegex.Match(soapMessageBody);

            return match.Success ? match.Value : string.Empty;
        }

        public static string ParseXRoadMessageId(XDocument soapMessage)
        {
            return soapMessage.Descendants()
                .First(tag =>
                    tag.Name.LocalName.Equals("id") &&
                    tag.Name.NamespaceName.Equals("http://x-road.eu/xsd/xroad.xsd"))
                .Value;
        }

        public static string ParseXRoadMessageIssue(XDocument soapMessage)
        {
            return soapMessage.Descendants()
                .First(tag =>
                    tag.Name.LocalName.Equals("issue") &&
                    tag.Name.NamespaceName.Equals("http://x-road.eu/xsd/xroad.xsd"))
                .Value;
        }

        public static string ParseXRoadUserId(XDocument soapMessage)
        {
            return soapMessage.Descendants()
                .First(tag =>
                    tag.Name.LocalName.Equals("userId") &&
                    tag.Name.NamespaceName.Equals("http://x-road.eu/xsd/xroad.xsd"))
                .Value;
        }

        public static XRoadService ParseXRoadService(XDocument soapMessage)
        {
            var service = soapMessage
                .Descendants()
                .First(tag =>
                    tag.Name.LocalName.Equals("service") &&
                    tag.Name.NamespaceName.Equals("http://x-road.eu/xsd/xroad.xsd"));

            // @formatter:off
            return new XRoadService
            {
                XRoadInstance = service.Descendants().First(tag => tag.Name.LocalName.Equals("xRoadInstance")).Value,
                MemberClass = service.Descendants().First(tag => tag.Name.LocalName.Equals("memberClass")).Value,
                MemberCode = service.Descendants().First(tag => tag.Name.LocalName.Equals("memberCode")).Value,
                SubsystemCode = service.Descendants().FirstOrDefault(tag => tag.Name.LocalName.Equals("subsystemCode"))?.Value,
                ServiceCode = service.Descendants().First(tag => tag.Name.LocalName.Equals("serviceCode")).Value,
                ServiceVersion = service.Descendants().FirstOrDefault(tag => tag.Name.LocalName.Equals("serviceVersion"))?.Value,
            };
            // @formatter:on
        }

        public static XRoadClient ParseXRoadClient(XDocument soapMessage)
        {
            var client = soapMessage.Descendants()
                .First(tag =>
                    tag.Name.LocalName.Equals("client") &&
                    tag.Name.NamespaceName.Equals("http://x-road.eu/xsd/xroad.xsd"));

            // @formatter:off
            return new XRoadClient
            {
                XRoadInstance = client.Descendants().First(tag => tag.Name.LocalName.Equals("xRoadInstance")).Value,
                MemberClass = client.Descendants().First(tag => tag.Name.LocalName.Equals("memberClass")).Value,
                MemberCode = client.Descendants().First(tag => tag.Name.LocalName.Equals("memberCode")).Value,
                SubsystemCode = client.Descendants().FirstOrDefault(tag => tag.Name.LocalName.Equals("subsystemCode"))?.Value,
            };
            // @formatter:on
        }
    }
}