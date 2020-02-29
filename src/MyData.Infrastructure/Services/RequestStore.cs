using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyData.Core.Interfaces;
using MyData.Core.Models;
using MyData.Infrastructure.EntityFrameworkCore;
using X.PagedList;

namespace MyData.Infrastructure.Services
{
    public class RequestStore : IRequestStore, IDisposable
    {
        private readonly AppDbContext _dbContext;

        public RequestStore(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task AddRangeAsync(Request[] requests)
        {
            await _dbContext.Requests.AddRangeAsync(requests);
            await _dbContext.SaveChangesAsync();
        }

        public Task<IPagedList<Request>> SearchAsync(DateTime @from, DateTime to, int pageNumber, int pageSize)
        {
            return _dbContext.Requests
                .Where(request => request.ServiceInvokedAt >= from && request.ServiceInvokedAt <= to)
                .OrderBy(request => request.Id)
                .ToPagedListAsync(pageNumber, pageSize);
        }

        public Task<int> PurgeAsync(DateTime @from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}