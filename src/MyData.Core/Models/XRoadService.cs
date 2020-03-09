using System;
using System.Text;

namespace MyData.Core.Models
{
    public class XRoadService
    {
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string XRoadInstance { get; set; }

        public string MemberClass { get; set; }

        public string MemberCode { get; set; }

        /// <summary>
        /// can be optional
        /// </summary>
        public string SubsystemCode { get; set; }

        public string ServiceCode { get; set; }

        /// <summary>
        /// for rest services it is always null
        /// </summary>
        public string ServiceVersion { get; set; }

        public bool IsRestService { get; set; }

        public bool SameAs(XRoadService service)
        {
            return XRoadInstance.Equals(service.XRoadInstance)
                   && MemberClass.Equals(service.MemberClass)
                   && MemberCode.Equals(service.MemberCode)
                   && ServiceCode.Equals(service.ServiceCode)
                   && string.Equals(SubsystemCode, service.SubsystemCode)
                   && string.Equals(ServiceVersion, service.ServiceVersion);
        }

        public bool RestPathMatches(string path)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"/{XRoadInstance}/{MemberClass}/{MemberCode}");
            if (!string.IsNullOrEmpty(SubsystemCode))
            {
                stringBuilder.Append($"/{SubsystemCode}");
            }

            stringBuilder.Append($"/{ServiceCode}");
            var restServicePath = stringBuilder.ToString();
            return path.Contains(restServicePath,StringComparison.InvariantCultureIgnoreCase);
        }
    }
}