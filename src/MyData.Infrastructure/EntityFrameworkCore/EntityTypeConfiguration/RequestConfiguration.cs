using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyData.Core.Models;

namespace MyData.Infrastructure.EntityFrameworkCore.EntityTypeConfiguration
{
    public class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.HasKey(request => request.Id);
            
            builder.Property(request => request.CreatedAt)
                .HasValueGenerator<CurrentDateTimeGenerator>()
                .ValueGeneratedOnAdd();
        }
    }
}