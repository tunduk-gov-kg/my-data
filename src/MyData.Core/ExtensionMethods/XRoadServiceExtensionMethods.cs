using MyData.Core.Models;

namespace MyData.Core.ExtensionMethods
{
    public static class XRoadServiceExtensionMethods
    {
        public static bool SameAs(this XRoadService firstInstance, XRoadService secondInstance)
        {
            return firstInstance.XRoadInstance.Equals(secondInstance.XRoadInstance)
                   && firstInstance.MemberClass.Equals(secondInstance.MemberClass)
                   && firstInstance.MemberCode.Equals(secondInstance.MemberCode)
                   && firstInstance.ServiceCode.Equals(secondInstance.ServiceCode)
                   && string.Equals(firstInstance.SubsystemCode, secondInstance.SubsystemCode)
                   && string.Equals(firstInstance.ServiceVersion, secondInstance.ServiceVersion);
        }
    }
}