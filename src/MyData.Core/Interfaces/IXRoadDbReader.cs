using System.Collections.Generic;
using MyData.Core.Models;

namespace MyData.Core.Interfaces
{
    public interface IXRoadDbReader
    {
        List<XRoadRequest> Read(XRoadLogsDb sourceDb, long fromIdInclusive, long toIdInclusive);
    }
}