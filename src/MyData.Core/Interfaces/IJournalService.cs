using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyData.Core.Models;

namespace MyData.Core.Interfaces
{
    public interface IJournalService
    {
        Task AddAsync(JournalRecord journalRecord);

        Task<List<JournalRecord>> SearchAsync(DateTime fromInclusive, DateTime toInclusive);

        Task<JournalRecord> GetLastSucceededOperationAsync(string dbHost);
    }
}