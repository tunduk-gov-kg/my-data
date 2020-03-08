using System;
using MyData.Core.Models;

namespace MyData.Infrastructure.XRoad
{
    public static class XRoadUtils
    {
        public static XRoadRequest ParseSoap(XRoadLog xRoadLog)
        {
            throw new NotImplementedException();
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