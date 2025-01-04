using Data.Entities.CommonEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configs.CommonConfig
{
    public class SummaryConfiguration : IEntityTypeConfiguration<Summary>
    {
        public void Configure(EntityTypeBuilder<Summary> builder)
        {
            builder.ToTable("Summaries");
            builder.Property(e => e.Date).HasColumnType("date");
            builder.HasKey(e => e.Date);
        }
    }
}
