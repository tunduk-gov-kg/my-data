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
    public class JournalService : IJournalService, IDisposable
    {
        private readonly AppDbContext _dbContext;

        public JournalService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(JournalRecord journalRecord)
        {
            await _dbContext.Journal.AddAsync(journalRecord);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<JournalRecord>> SearchAsync(DateTime fromInclusive, DateTime toInclusive)
        {
            return await _dbContext.Journal.OrderBy(log => log.Id)
                .Where(log => log.CreatedAt >= fromInclusive)
                .Where(log => log.CreatedAt <= toInclusive)
                .ToListAsync();
        }

        public Task<JournalRecord> GetLastSucceededOperationAsync(string dbHost)
        {
            return _dbContext.Journal.OrderBy(log => log.Id)
                .Where(log => log.Succeeded)
                .LastOrDefaultAsync(log => log.DbHost.Equals(dbHost));
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}