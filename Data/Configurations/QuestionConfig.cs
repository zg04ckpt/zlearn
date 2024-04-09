using ZG04WEB.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZG04WEB.Data.Configurations
{
    public class QuestionConfig : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Content).IsRequired().HasMaxLength(1000);
            builder.Property(x => x.AnswerA).HasMaxLength(500);
            builder.Property(x => x.AnswerB).HasMaxLength(500);
            builder.Property(x => x.AnswerC).HasMaxLength(500);
            builder.Property(x => x.AnswerD).HasMaxLength(500);
            builder.Property(x => x.CorrectAnswer).IsRequired();

            builder.HasOne(x => x.QuestionSet)
                .WithMany(x => x.Questions)
                .HasForeignKey(x => x.QuestionSetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
