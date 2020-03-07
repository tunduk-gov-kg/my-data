using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyData.Core.Interfaces;
using MyData.Core.Models;
using MyData.Infrastructure.EntityFrameworkCore;

namespace MyData.Infrastructure.Services
{
    public class LogStore : ILogStore, IDisposable
    {
        private readonly AppDbContext _dbContext;

        public LogStore(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Log log)
        {
            await _dbContext.Logs.AddAsync(log);
        }

        public async Task<List<Log>> SearchAsync(DateTime fromInclusive, DateTime toInclusive)
        {
            return await _dbContext.Logs.OrderBy(log => log.Id)
                .Where(log => log.CreatedAt >= fromInclusive)
                .Where(log => log.CreatedAt <= toInclusive)
                .ToListAsync();
        }

        public Task<Log> LastOrDefaultAsync(string dbHost)
        {
            return _dbContext.Logs.OrderBy(log => log.Id).LastOrDefaultAsync(log => log.DbHost.Equals(dbHost));
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}