using System.Collections.Generic;
using MyData.Core.Models;

namespace MyData.Core.Interfaces
{
    public interface IXRoadLogsDbListProvider
    {
        public List<XRoadLogsDb> List { get; }
    }
}