using System;

namespace MyData.Core.Models
{
    public class Service
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string XRoadInstance { get; set; }
        public string MemberClass { get; set; }
        public string MemberCode { get; set; }
        public string SubsystemCode { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceVersion { get; set; }
    }
}