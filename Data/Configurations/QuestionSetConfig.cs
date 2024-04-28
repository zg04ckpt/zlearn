using  Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace  Data.Configurations
{
    public class QuestionSetConfig : IEntityTypeConfiguration<QuestionSet>
    {
        public void Configure(EntityTypeBuilder<QuestionSet> builder)
        {
            builder.ToTable("QuestionSets");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(1000);
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.UpdatedDate).IsRequired();
            builder.Property(x => x.ImageUrl).HasMaxLength(500);
        }
    }
}
