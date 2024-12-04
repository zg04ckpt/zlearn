using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configs
{
    public class SummaryConfig : IEntityTypeConfiguration<Summary>
    {
        public void Configure(EntityTypeBuilder<Summary> builder)
        {
            builder.ToTable("Summaries");
            builder.Property(e => e.Date).HasColumnType("date");
            builder.HasKey(e => e.Date);
        }
    }
}
