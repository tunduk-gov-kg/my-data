using System;

namespace MyData.Core.Models
{
    public class Log
    {
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string DbHost { get; set; }

        public string DbPort { get; set; }

        public long FromIdInclusive { get; set; }

        public long Limit { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorDescription { get; set; }

        public bool Succeeded { get; set; }
    }
}