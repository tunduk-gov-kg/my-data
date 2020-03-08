using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyData.Core.Models;

namespace MyData.Infrastructure.EntityFrameworkCore.EntityTypeConfiguration
{
    public class XRoadServiceConfiguration : IEntityTypeConfiguration<XRoadService>
    {
        public void Configure(EntityTypeBuilder<XRoadService> builder)
        {
            builder.HasKey(service => service.Id);
            builder.Property(service => service.CreatedAt)
                .HasValueGenerator<CurrentDateTimeGenerator>()
                .ValueGeneratedOnAdd();
        }
    }
}