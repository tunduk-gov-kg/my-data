using MyData.Core.Models;

namespace MyData.Core.Interfaces
{
    public interface IXRoadDbReader
    {
        XRoadLogsReadResult Read(XRoadLogsDb sourceDb, long fromIdInclusive, int limit);

        bool AnyRecords(XRoadLogsDb sourceDb, long fromIdInclusive);
    }
}