using System.Collections.Generic;
using System.Linq;

// ReSharper disable RedundantIfElseBlock

namespace MyData.Core.Models
{
    public class XRoadLogsReadResult : List<XRoadLog>
    {
        public long LastRecordId
        {
            get { return this.Max(log => log.Id); }
        }
    }
}