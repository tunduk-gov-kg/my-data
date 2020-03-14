using System;

namespace MyData.Core.Models
{
    public class JournalRecord
    {
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string DbHost { get; set; }

        public int DbPort { get; set; }

        public long FromIdInclusive { get; set; }

        public long ToIdInclusive { get; set; }
        
        public int ActualCount { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorDescription { get; set; }

        public bool Succeeded { get; set; }
    }
}