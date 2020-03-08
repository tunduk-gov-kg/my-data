using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyData.Core.Models;

namespace MyData.Infrastructure.EntityFrameworkCore.EntityTypeConfiguration
{
    public class JournalRecordConfiguration : IEntityTypeConfiguration<JournalRecord>
    {
        public void Configure(EntityTypeBuilder<JournalRecord> builder)
        {
            builder.HasKey(log => log.Id);
            
            builder.Property(log => log.CreatedAt)
                .HasValueGenerator<CurrentDateTimeGenerator>()
                .ValueGeneratedOnAdd();
        }
    }
}