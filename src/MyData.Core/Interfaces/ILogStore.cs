using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyData.Core.Models;

namespace MyData.Core.Interfaces
{
    public interface ILogStore
    {
        public Task AddAsync(Log log);

        public Task<List<Log>> SearchAsync(DateTime fromInclusive, DateTime toInclusive);

        public Task<Log> LastOrDefaultAsync(string dbHost);
    }
}