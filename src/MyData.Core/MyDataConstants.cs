using System.Text.RegularExpressions;

namespace MyData.Core
{
    public static class MyDataConstants
    {
        private const string KgzPinPattern = @"[012]{1}[0,1,2,3]{1}[0-9]{1}[0,1]{1}[0-9]{1}[1,2]{1}[0-9]{3}[0-9]{5}";

        private const string XRoadRestServiceCodePattern =
            @"\/r1\/[a-zA-Z-]{2,14}\/[a-zA-Z-]{2,3}\/[0-9]{2,8}\/[a-zA-Z-_\/0-9]+";

        private const string XRoadRestClientPattern = @"x-road-client:[a-zA-Z-]{2,14}\/[a-zA-Z-]{2,3}\/[0-9]{2,8}.*";

        private const string XRoadRequestIdPattern = @"x-road-request-id:.*";

        private const string XRoadIdPattern = @"x-road-id:.*";

        private const string XRoadUserIdPattern = @"x-road-userid:.*";

        private const string XRoadIssuePattern = @"x-road-issue:.*";

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