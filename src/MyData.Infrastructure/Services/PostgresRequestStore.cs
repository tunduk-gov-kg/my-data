using System;
using System.Linq;
using System.Threading.Tasks;
using MyData.Core.Interfaces;
using MyData.Core.Models;
using MyData.Infrastructure.EntityFrameworkCore;
using X.PagedList;

namespace MyData.Infrastructure.Services
{
    public class PostgresRequestStore : IRequestStore, IDisposable
    {
        private readonly AppDbContext _dbContext;

        public PostgresRequestStore(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRangeAsync(Request[] requests)
        {
            await _dbContext.Requests.AddRangeAsync(requests);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IPagedList<Request>> SearchAsync(DateTime @from, DateTime to, int pageNumber, int pageSize)
        {
            return await _dbContext.Requests
                .Where(request => request.ServiceInvokedAt >= from && request.ServiceInvokedAt <= to)
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