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
    public class XRoadServiceStore : IXRoadServiceStore, IDisposable
    {
        private readonly AppDbContext _dbContext;

        public XRoadServiceStore(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private Task<List<XRoadService>> GetListAsync()
        {
            return _dbContext.XRoadServices.ToListAsync();
        }

        public Task<List<XRoadService>> GetRestServicesAsync()
        {
            return _dbContext.XRoadServices
                .Where(service => service.IsRestService)
                .ToListAsync();
        }

        public Task<List<XRoadService>> GetSoapServicesAsync()
        {
            return _dbContext.XRoadServices
                .Where(service => !service.IsRestService)
                .ToListAsync();
        }

        public async Task UpdateListAsync(List<XRoadService> newList)
        {
            var oldList = await GetListAsync();

            var forRemove = oldList.Where(oldListService =>
                newList.All(newListService => !oldListService.SameAs(newListService))).ToList();

            var forAdd = newList.Where(newListService =>
                oldList.All(oldListService => !newListService.SameAs(oldListService))).ToList();

            _dbContext.XRoadServices.RemoveRange(forRemove);
            _dbContext.XRoadServices.AddRange(forAdd);
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}