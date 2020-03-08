using System.Collections.Generic;
using System.Threading.Tasks;
using MyData.Core.Models;

namespace MyData.Core.Interfaces
{
    public interface IXRoadServiceStore
    {
        Task<List<XRoadService>> GetListAsync();
        
        // ReSharper disable once ParameterTypeCanBeEnumerable.Global
        Task UpdateListAsync(List<XRoadService> newList);
    }
}