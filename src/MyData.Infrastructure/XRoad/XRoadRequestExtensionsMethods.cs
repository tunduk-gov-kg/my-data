using MyData.Core.Models;

namespace MyData.Infrastructure.XRoad
{
    public static class XRoadRequestExtensionsMethods
    {
        public static void SetXRoadService(this XRoadRequest request, XRoadService xRoadService)
        {
            request.ServiceXRoadInstance = xRoadService.XRoadInstance;
            request.ServiceMemberClass = xRoadService.MemberClass;
            request.ServiceMemberCode = xRoadService.MemberCode;
            request.ServiceSubsystemCode = xRoadService.SubsystemCode;
            request.ServiceCode = xRoadService.ServiceCode;
            request.ServiceVersion = xRoadService.ServiceVersion;
        }

        public static void SetXRoadClient(this XRoadRequest request, XRoadClient xRoadClient)
        {
            request.ClientXRoadInstance = xRoadClient.XRoadInstance;
            request.ClientMemberClass = xRoadClient.MemberClass;
            request.ClientMemberCode = xRoadClient.MemberCode;
            request.ClientSubsystemCode = xRoadClient.SubsystemCode;
        }
        
        
    }
}