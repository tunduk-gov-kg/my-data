using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace MyData.Infrastructure.EntityFrameworkCore
{
    public class CurrentDateTimeGenerator : ValueGenerator<DateTime>
    {
        public override DateTime Next(EntityEntry entry) => DateTime.Now;
        
        public override bool GeneratesTemporaryValues => false;
    }
}