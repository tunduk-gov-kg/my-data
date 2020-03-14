using System.Collections.Generic;
using System.Linq;

// ReSharper disable RedundantIfElseBlock

namespace MyData.Core.Models
{
    public class XRoadLogsReadResult
    {
        public XRoadLogsReadResult(long fromIdInclusive, long toIdInclusive)
        {
            FromIdInclusive = fromIdInclusive;
            ToIdInclusive = toIdInclusive;
        }

        public List<XRoadLog> XRoadLogs { get; set; } = new List<XRoadLog>();

        private long FromIdInclusive { get; set; }

        private long ToIdInclusive { get; set; }

        public bool IsLast => XRoadLogs.Count == 0 || LastRecordId != ToIdInclusive;

        public long NextToIdInclusive => LastRecordId + 1;

        public long LastRecordId
        {
            get { return XRoadLogs.Max(log => log.Id); }
        }
    }
}