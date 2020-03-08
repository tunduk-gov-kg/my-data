using System;
using System.Threading.Tasks;
using MyData.Core.Models;
using X.PagedList;

namespace MyData.Core.Interfaces
{
    public interface IXRoadRequestStore
    {
        Task AddRangeAsync(XRoadRequest[] requests);

        Task<IPagedList<XRoadRequest>> SearchAsync(DateTime fromInclusive, DateTime toInclusive, int pageNumber,
            int pageSize);

        Task<int> PurgeAsync(DateTime fromInclusive, DateTime toInclusive);
    }
}