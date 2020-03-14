using System.Collections.Generic;
using MyData.Core.Models;

namespace MyData.Core.Interfaces
{
    public interface IXRoadLogsProcessor
    {
        List<XRoadRequest> Process(List<XRoadLog> logs);
    }
}