using System.Collections.Generic;
using System.Threading.Tasks;
using MyData.Core.Models;

namespace MyData.Core.Interfaces
{
    public interface IServiceStore
    {
        Task<List<Service>> GetListAsync();
        
        Task UpdateListAsync(List<Service> newList);
    }
}