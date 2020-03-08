using System.Collections.Generic;
using MyData.Core.Interfaces;
using MyData.Core.Models;

namespace MyData.Infrastructure.Services
{
    public class InMemoryXRoadLogsDbListProvider : IXRoadLogsDbListProvider
    {
        public InMemoryXRoadLogsDbListProvider(List<XRoadLogsDb> list)
        {
            List = list;
        }

        public List<XRoadLogsDb> List { get; }
    }
}