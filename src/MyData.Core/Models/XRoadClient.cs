using System;

namespace MyData.Core.Models
{
    public class XRoadClient
    {
        public string XRoadInstance { get; set; }

        public string MemberClass { get; set; }

        public string MemberCode { get; set; }

        public string SubsystemCode { get; set; }

        protected bool Equals(XRoadClient other)
        {
            return XRoadInstance == other.XRoadInstance && MemberClass == other.MemberClass &&
                   MemberCode == other.MemberCode && SubsystemCode == other.SubsystemCode;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((XRoadClient) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(XRoadInstance, MemberClass, MemberCode, SubsystemCode);
        }

        public static XRoadClient From(string restPath)
        {
            var identifiers = restPath.Split('/');

            return new XRoadClient
            {
                XRoadInstance = identifiers[0],
                MemberClass = identifiers[1],
                MemberCode = identifiers[2],
                SubsystemCode = identifiers.Length == 4 ? identifiers[3] : null
            };
        }
    }
}
