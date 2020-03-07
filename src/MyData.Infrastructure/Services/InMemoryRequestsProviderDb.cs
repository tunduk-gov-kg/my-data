using System.Collections.Generic;
using MyData.Core.Interfaces;
using MyData.Core.Models;

namespace MyData.Infrastructure.Services
{
    public class InMemoryRequestsProviderDb : IRequestsProviderDb
    {
        public InMemoryRequestsProviderDb(List<RequestsProviderDb> list)
        {
            List = list;
        }

        public List<RequestsProviderDb> List { get; }
    }
}