using  Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace  Data.Configurations
{
    public class TestConfig : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.ToTable("Tests");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(1000);
            builder.Property(x => x.Source).IsRequired().HasMaxLength(500);
            builder.Property(x => x.UpdatedDate).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.ImageUrl).IsRequired(false);
            builder.Property(x => x.Duration).IsRequired();
            builder.Property(x => x.NumberOfQuestions).IsRequired();
            builder.Property(x => x.NumberOfAttempts).IsRequired();
            builder.Property(x => x.AuthorName).IsRequired();

            builder.HasOne(x => x.Author)
                .WithMany(x => x.Tests)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
