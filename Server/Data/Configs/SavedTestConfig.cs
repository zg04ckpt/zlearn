using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configs
{
    public class SavedTestConfig : IEntityTypeConfiguration<SavedTest>
    {
        public void Configure(EntityTypeBuilder<SavedTest> builder)
        {
            builder.ToTable("SavedTests");
            builder.HasKey(x => new {x.UserId, x.TestId});
            builder
                .HasOne(x => x.User)
                .WithMany(x => x.UserInTests)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(x => x.Test)
                .WithMany(x => x.UserInTests)
                .HasForeignKey(x => x.TestId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
