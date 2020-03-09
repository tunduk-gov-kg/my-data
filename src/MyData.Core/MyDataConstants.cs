using System.Text.RegularExpressions;

namespace MyData.Core
{
    public static class MyDataConstants
    {
        public const string KgzPinPattern = @"[012]{1}[0,1,2,3]{1}[0-9]{1}[0,1]{1}[0-9]{1}[1,2]{1}[0-9]{3}[0-9]{5}";

        public const string XRoadRestServiceCodePattern =
            @"\/r1\/[a-zA-Z-]{2,14}\/[a-zA-Z-]{2,3}\/[0-9]{2,8}\/[a-zA-Z-_\/0-9]+";

        public const string XRoadRestClientPattern = @"X-Road-Client:[[a-zA-Z-]{2,14}\/[a-zA-Z-]{2,3}\/[0-9]{2,8}.*";

        public const string XRoadRequestIdPattern = @"x-road-request-id:.*";

        public const string XRoadIdPattern = @"x-road-id:.*";

        public const string XRoadUserIdPattern = @"x-road-userid:.*";

        public const string XRoadIssuePattern = @"x-road-issue:.*";

        public static class RegEx
        {
            public static readonly Regex KgzPinRegex = new Regex(KgzPinPattern);

            public static readonly Regex XRoadRestServiceCodeRegex = new Regex(XRoadRestServiceCodePattern);

            public static readonly Regex XRoadRestClientRegex = new Regex(XRoadRestClientPattern);

            public static readonly Regex XRoadRequestIdRegex = new Regex(XRoadRequestIdPattern);

            public static readonly Regex XRoadIdRegex = new Regex(XRoadIdPattern);

            public static readonly Regex XRoadUserIdRegex = new Regex(XRoadUserIdPattern);

            public static readonly Regex XRoadIssueRegex = new Regex(XRoadIssuePattern);
        }
    }
}