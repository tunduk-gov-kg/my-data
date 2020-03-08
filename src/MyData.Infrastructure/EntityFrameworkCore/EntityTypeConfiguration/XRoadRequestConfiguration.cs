using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyData.Core.Models;

namespace MyData.Infrastructure.EntityFrameworkCore.EntityTypeConfiguration
{
    public class XRoadRequestConfiguration : IEntityTypeConfiguration<XRoadRequest>
    {
        public void Configure(EntityTypeBuilder<XRoadRequest> builder)
        {
            builder.HasKey(request => request.Id);
            
            builder.Property(request => request.CreatedAt)
                .HasValueGenerator<CurrentDateTimeGenerator>()
                .ValueGeneratedOnAdd();
        }
    }
}