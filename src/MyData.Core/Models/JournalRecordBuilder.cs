using System;

namespace MyData.Core.Models
{
    public class JournalRecordBuilder
    {
        private readonly JournalRecord _journalRecord = new JournalRecord()
        {
            Succeeded = true
        };

        public JournalRecordBuilder SetXRoadDb(XRoadLogsDb logsDb)
        {
            _journalRecord.DbHost = logsDb.Host;
            _journalRecord.DbPort = logsDb.Port;
            return this;
        }

        public JournalRecordBuilder SetFromIdInclusive(long fromIdInclusive)
        {
            _journalRecord.FromIdInclusive = fromIdInclusive;
            return this;
        }

        public JournalRecordBuilder SetLimit(int limit)
        {
            _journalRecord.Limit = limit;
            return this;
        }

        public JournalRecordBuilder SetLastRecordId(long lastRecordId)
        {
            _journalRecord.LastRecordId = lastRecordId;
            return this;
        }
        
        public JournalRecordBuilder SetParsedCount(int parsedCount)
        {
            _journalRecord.ParsedCount = parsedCount;
            return this;
        }

        public JournalRecordBuilder SetException(Exception exception)
        {
            _journalRecord.Succeeded = false;
            _journalRecord.ErrorCode = exception.GetType().FullName;
            _journalRecord.ErrorDescription = exception.Message;
            return this;
        }

        public JournalRecord Build()
        {
            var result = _journalRecord;
            return result;
        }
    }
}