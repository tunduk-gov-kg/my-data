using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyData.Core.Models;

namespace MyData.Core.Interfaces
{
    public interface IJournalService
    {
        public Task AddAsync(JournalRecord journalRecord);

        public Task<List<JournalRecord>> SearchAsync(DateTime fromInclusive, DateTime toInclusive);

        public Task<JournalRecord> LastOrDefaultAsync(string dbHost);
    }
}