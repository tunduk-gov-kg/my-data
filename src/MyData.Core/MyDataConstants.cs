using System.Text.RegularExpressions;

namespace MyData.Core
{
    public static class MyDataConstants
    {   
        private const string KgzPinPattern = @"[012]{1}[0,1,2,3]{1}[0-9]{1}[0,1]{1}[0-9]{1}[1,2]{1}[0-9]{3}[0-9]{5}";

        private const string XRoadRestServiceCodePattern =
            @"\/r1\/[a-zA-Z-]{2,14}\/[a-zA-Z-]{2,3}\/[0-9]{2,8}\/[a-zA-Z-_\/0-9]+";

        private const string XRoadRestClientPattern = @"(?i)x-road-client(?-i):[a-zA-Z-]{2,14}\/[a-zA-Z-]{2,3}\/[0-9]{2,8}.*";
        
        private const string XRoadIdPattern = @"(?i)x-road-id(?-i):.*";

        private const string XRoadUserIdPattern = @"(?i)x-road-userid(?-i):.*";

        private const string XRoadIssuePattern = @"(?i)x-road-issue(?-i):.*";

        public const int KgzPinLength = 14;

        public static class RegEx
        {
            public static readonly Regex KgzPinRegex = new Regex(KgzPinPattern);

            public static readonly Regex XRoadRestServiceCodeRegex = new Regex(XRoadRestServiceCodePattern);

            public static readonly Regex XRoadRestClientRegex = new Regex(XRoadRestClientPattern);
            
            public static readonly Regex XRoadIdRegex = new Regex(XRoadIdPattern);

            public static readonly Regex XRoadUserIdRegex = new Regex(XRoadUserIdPattern);

            public static readonly Regex XRoadIssueRegex = new Regex(XRoadIssuePattern);
        }
    }
}