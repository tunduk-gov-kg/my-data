using System;

namespace MyData.Core.Models
{
    public class XRoadRequest
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }

        // X-Road metadata
        public DateTime ServiceInvokedAt { get; set; }

        public string ClientXRoadInstance { get; set; }
        public string ClientMemberClass { get; set; }
        public string ClientMemberCode { get; set; }
        public string ClientSubsystemCode { get; set; }

        public string ServiceXRoadInstance { get; set; }
        public string ServiceMemberClass { get; set; }
        public string ServiceMemberCode { get; set; }
        public string ServiceSubsystemCode { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceVersion { get; set; }

        public string XRequestId { get; set; }
        public string MessageId { get; set; }
        public string UserId { get; set; }
        public string MessageIssue { get; set; }

        //Personal identification number passed as parameter to target service
        public string Pin { get; set; }


        public XRoadService XRoadService =>
            new XRoadService
            {
                XRoadInstance = ServiceXRoadInstance,
                MemberClass = ServiceMemberClass,
                MemberCode = ServiceMemberCode,
                SubsystemCode = ServiceSubsystemCode,
                ServiceCode = ServiceCode,
                ServiceVersion = ServiceVersion
            };
    }
}