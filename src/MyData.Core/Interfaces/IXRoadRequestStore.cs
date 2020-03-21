using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyData.Core.Models;
using X.PagedList;

namespace MyData.Core.Interfaces
{
    public interface IXRoadRequestStore
    {
        Task AddRangeAsync(List<XRoadRequest> requests);

        Task<IPagedList<XRoadRequest>> SearchAsync(DateTime fromInclusive, DateTime toInclusive, int pageNumber,
            int pageSize);

        Task<int> PurgeAsync(DateTime untilInclusive);
    }
}