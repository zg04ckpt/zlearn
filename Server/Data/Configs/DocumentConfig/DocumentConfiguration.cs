using Data.Entities.DocumentEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configs.DocumentConfig
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable("Documents");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(255);
            builder.Property(x => x.FileName).HasMaxLength(150);
            builder.Property(x => x.Slug).HasMaxLength(100);
            builder.Property(x => x.CategorySlug).HasMaxLength(100);
            builder.Property(x => x.ImageUrl).IsRequired(false).HasMaxLength(200);

            builder.HasOne(x => x.Author)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PaymentInfo)
                .WithOne(x => x.Document)
                .HasForeignKey<Document>(x => x.PaymentInfoId).IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
