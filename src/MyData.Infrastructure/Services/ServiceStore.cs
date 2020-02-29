using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyData.Core.Interfaces;
using MyData.Core.Models;
using MyData.Infrastructure.EntityFrameworkCore;
using X.PagedList;

namespace MyData.Infrastructure.Services
{
    public class ServiceStore : IServiceStore, IDisposable
    {
        private readonly AppDbContext _dbContext;

        public ServiceStore(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Service>> GetListAsync()
        {
            return _dbContext.Services.ToListAsync();
        }

        public async Task UpdateListAsync(List<Service> newList)
        {
            var oldList = await GetListAsync();

            var forRemove = oldList.Where(oldListService =>
                newList.All(newListService => !SameAs(oldListService, newListService))).ToList();

            var forAdd = newList.Where(newListService =>
                oldList.All(oldListService => !SameAs(newListService, oldListService))).ToList();

            _dbContext.Services.RemoveRange(forRemove);
            _dbContext.Services.AddRange(forAdd);
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        private static bool SameAs(Service firstInstance, Service secondInstance)
        {
            return firstInstance.XRoadInstance.Equals(secondInstance.XRoadInstance)
                   && firstInstance.MemberClass.Equals(secondInstance.MemberClass)
                   && firstInstance.MemberCode.Equals(secondInstance.MemberCode)
                   && firstInstance.ServiceCode.Equals(secondInstance.ServiceCode)
                   && string.Equals(firstInstance.SubsystemCode, secondInstance.SubsystemCode)
                   && string.Equals(firstInstance.ServiceVersion, secondInstance.ServiceVersion);
        }
    }
}