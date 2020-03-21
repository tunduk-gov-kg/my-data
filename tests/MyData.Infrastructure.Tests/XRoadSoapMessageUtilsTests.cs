using System.Xml.Linq;
using MyData.Infrastructure.XRoad;
using Xunit;

namespace MyData.Infrastructure.Tests
{
    public class XRoadSoapMessageUtilsTests
    {
        private readonly string RequestWithoutPin =
            "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Header><h:client a:objectType=\"SUBSYSTEM\" xmlns:h=\"http://x-road.eu/xsd/xroad.xsd\" xmlns:a=\"http://x-road.eu/xsd/identifiers\" xmlns=\"http://x-road.eu/xsd/xroad.xsd\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><a:xRoadInstance>DEV</a:xRoadInstance><a:memberClass>COM</a:memberClass><a:memberCode>60000002</a:memberCode><a:subsystemCode>MICROSOFT_SUBSYSTEM</a:subsystemCode></h:client><h:id xmlns:h=\"http://x-road.eu/xsd/xroad.xsd\" xmlns=\"http://x-road.eu/xsd/xroad.xsd\">d08f3a77-98f3-400f-9f80-b7329ab93ccc</h:id><h:issue xmlns:h=\"http://x-road.eu/xsd/xroad.xsd\" xmlns=\"http://x-road.eu/xsd/xroad.xsd\">Sample Issue</h:issue><h:protocolVersion xmlns:h=\"http://x-road.eu/xsd/xroad.xsd\" xmlns=\"http://x-road.eu/xsd/xroad.xsd\">4.0</h:protocolVersion><h:service a:objectType=\"SERVICE\" xmlns:h=\"http://x-road.eu/xsd/xroad.xsd\" xmlns:a=\"http://x-road.eu/xsd/identifiers\" xmlns=\"http://x-road.eu/xsd/xroad.xsd\"><a:xRoadInstance>DEV</a:xRoadInstance><a:memberClass>COM</a:memberClass><a:memberCode>60000001</a:memberCode><a:subsystemCode>GOOGLE_SUBSYSTEM</a:subsystemCode><a:serviceCode>getRandom</a:serviceCode><a:serviceVersion>v1</a:serviceVersion></h:service><h:userId xmlns:h=\"http://x-road.eu/xsd/xroad.xsd\" xmlns=\"http://x-road.eu/xsd/xroad.xsd\">CONSUMER_ID</h:userId></s:Header><s:Body xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><getRandom xmlns=\"http://test.x-road.fi/producer\"/></s:Body></s:Envelope>";

        private readonly string BadRequest = "<invalid></invalid>";

        private readonly string RequestWithPin =
            "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Header><h:client a:objectType=\"SUBSYSTEM\" xmlns:h=\"http://x-road.eu/xsd/xroad.xsd\" xmlns:a=\"http://x-road.eu/xsd/identifiers\" xmlns=\"http://x-road.eu/xsd/xroad.xsd\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><a:xRoadInstance>DEV</a:xRoadInstance><a:memberClass>COM</a:memberClass><a:memberCode>60000002</a:memberCode><a:subsystemCode>MICROSOFT_SUBSYSTEM</a:subsystemCode></h:client><h:id xmlns:h=\"http://x-road.eu/xsd/xroad.xsd\" xmlns=\"http://x-road.eu/xsd/xroad.xsd\">d08f3a77-98f3-400f-9f80-b7329ab93ccc</h:id><h:issue xmlns:h=\"http://x-road.eu/xsd/xroad.xsd\" xmlns=\"http://x-road.eu/xsd/xroad.xsd\">Sample Issue</h:issue><h:protocolVersion xmlns:h=\"http://x-road.eu/xsd/xroad.xsd\" xmlns=\"http://x-road.eu/xsd/xroad.xsd\">4.0</h:protocolVersion><h:service a:objectType=\"SERVICE\" xmlns:h=\"http://x-road.eu/xsd/xroad.xsd\" xmlns:a=\"http://x-road.eu/xsd/identifiers\" xmlns=\"http://x-road.eu/xsd/xroad.xsd\"><a:xRoadInstance>DEV</a:xRoadInstance><a:memberClass>COM</a:memberClass><a:memberCode>60000001</a:memberCode><a:subsystemCode>GOOGLE_SUBSYSTEM</a:subsystemCode><a:serviceCode>getRandom</a:serviceCode><a:serviceVersion>v1</a:serviceVersion></h:service><h:userId xmlns:h=\"http://x-road.eu/xsd/xroad.xsd\" xmlns=\"http://x-road.eu/xsd/xroad.xsd\">20205199401330</h:userId></s:Header><s:Body xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><getRandom xmlns=\"http://test.x-road.fi/producer\">20205199401330</getRandom></s:Body></s:Envelope>";

        [Fact]
        public void Test1()
        {
            var xDocument = XDocument.Parse(RequestWithoutPin);
            var xRoadService = XRoadSoapMessageUtils.ParseXRoadService(xDocument);
            XRoadSoapMessageUtils.ParseXRoadClient(xDocument);
        }


        [Fact]
        public void Test2()
        {
            var xDocument = XDocument.Parse(RequestWithPin);
            XRoadSoapMessageUtils.ParsePin(xDocument,null);
            var xRoadClient = XRoadSoapMessageUtils.ParseXRoadClient(xDocument);
            ;
        }
    }
}