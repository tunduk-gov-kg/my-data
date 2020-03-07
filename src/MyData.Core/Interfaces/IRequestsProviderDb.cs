using System.Collections.Generic;
using MyData.Core.Models;

namespace MyData.Core.Interfaces
{
    public interface IRequestsProviderDb
    {
        public List<RequestsProviderDb> List { get; }
    }
}