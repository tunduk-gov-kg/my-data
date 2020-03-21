using System;

namespace MyData.Core.Models
{
    public class JournalRecord
    {
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string DbHost { get; set; }

        public int DbPort { get; set; }
        
        //where id >= fromIdInclusive
        public long FromIdInclusive { get; set; }

        //order by id limit @limit
        public int Limit { get; set; }
        
        public long LastRecordId { get; set; }
        
        public int ParsedCount { get; set; }
        
        public bool Succeeded { get; set; }
        
        public string ErrorCode { get; set; }

        public string ErrorDescription { get; set; }
    }
}