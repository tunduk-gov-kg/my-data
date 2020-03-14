using System.Text.RegularExpressions;
using MyData.Core;

namespace MyData.Infrastructure.XRoad
{
    public static class PinSearchUtil
    {
        public static string ParsePin(string parseText, string withPattern)
        {
            var allNumbers = Regex.Matches(parseText, "\\d+");

            foreach (Match numberMatch in allNumbers)
            {
                if (numberMatch.Value.Length != MyDataConstants.KgzPinLength)
                {
                    continue;
                }

                var pinMatchRegex = string.IsNullOrEmpty(withPattern)
                    ? MyDataConstants.RegEx.KgzPinRegex
                    : new Regex(withPattern);

                var pinMatch = pinMatchRegex.Match(numberMatch.Value);

                if (pinMatch.Success)
                {
                    return pinMatch.Value;
                }
            }

            return null;
        }
    }
}