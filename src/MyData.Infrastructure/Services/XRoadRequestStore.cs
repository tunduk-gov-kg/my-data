using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyData.Core.Interfaces;
using MyData.Core.Models;
using MyData.Infrastructure.EntityFrameworkCore;
using X.PagedList;

namespace MyData.Infrastructure.Services
{
    public class XRoadRequestStore : IXRoadRequestStore, IDisposable
    {
        private readonly AppDbContext _dbContext;

        public XRoadRequestStore(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task AddRangeAsync(List<XRoadRequest> requests)
        {
            await _dbContext.XRoadRequests.AddRangeAsync(requests);
            await _dbContext.SaveChangesAsync();
        }

        public Task<IPagedList<XRoadRequest>> SearchAsync(DateTime @from, DateTime to, int pageNumber, int pageSize)
        {
            return _dbContext.XRoadRequests
                .Where(request => request.ServiceInvokedAt >= from && request.ServiceInvokedAt <= to)
                .OrderBy(request => request.Id)
                .ToPagedListAsync(pageNumber, pageSize);
        }

        public Task<int> PurgeAsync(DateTime untilInclusive)
        {
            //npgsql command
            return _dbContext.Database.ExecuteSqlRawAsync(
                $"delete from \"XRoadRequests\" where \"CreatedAt\" <= '{untilInclusive.Date:O}'");
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}