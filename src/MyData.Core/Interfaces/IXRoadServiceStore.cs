using System.Collections.Generic;
using System.Threading.Tasks;
using MyData.Core.Models;

namespace MyData.Core.Interfaces
{
    public interface IXRoadServiceStore
    {
        Task<List<XRoadService>> GetListAsync();

        Task<List<XRoadService>> GetRestServicesAsync();

        Task<List<XRoadService>> GetSoapServicesAsync();

        // ReSharper disable once ParameterTypeCanBeEnumerable.Global
        Task UpdateListAsync(List<XRoadService> newList);
    }
}