using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MyData.Core;
using MyData.Core.Models;

namespace MyData.Infrastructure.XRoad
{
    public static class XRoadRestMessageUtils
    {
        public static Match MatchServiceRestPath(string xRoadRequest)
        {
            return MyDataConstants.RegEx.XRoadRestServiceCodeRegex.Match(xRoadRequest);
        }

        public static XRoadService FindMatchingRestPath(IEnumerable<XRoadService> restServices, string restPath)
        {
            return restServices.FirstOrDefault(service => service.RestPathMatches(restPath));
        }

        public static XRoadClient ParseXRoadClient(string xRoadRequest)
        {
            var clientMatch = MyDataConstants.RegEx.XRoadRestClientRegex.Match(xRoadRequest);
            if (!clientMatch.Success)
            {
                throw new ArgumentException("x-road-client header is required");
            }

            var restPath = clientMatch.Value.Replace("x-road-client:", string.Empty);
            return XRoadClient.From(restPath);
        }

        public static string ParsePin(string contextContent)
        {
            var pinMatch = MyDataConstants.RegEx.KgzPinRegex.Match(contextContent);
            return pinMatch.Success ? pinMatch.Value : null;
        }

        public static string ParseXRoadMessageId(string xRoadRequest)
        {
            var messageIdMatch = MyDataConstants.RegEx.XRoadIdRegex.Match(xRoadRequest);
            return messageIdMatch.Success ? messageIdMatch.Value.Replace("x-road-id:", string.Empty) : null;
        }

        public static string ParseXRoadMessageIssue(string xRoadRequest)
        {
            var issueMatch = MyDataConstants.RegEx.XRoadIssueRegex.Match(xRoadRequest);
            return issueMatch.Success ? issueMatch.Value.Replace("x-road-issue:", string.Empty) : null;
        }

        public static string ParseXRoadUserId(string xRoadRequest)
        {
            var userIdMatch = MyDataConstants.RegEx.XRoadUserIdRegex.Match(xRoadRequest);
            return userIdMatch.Success ? userIdMatch.Value.Replace("x-road-userid:", string.Empty) : null;
        }
    }
}