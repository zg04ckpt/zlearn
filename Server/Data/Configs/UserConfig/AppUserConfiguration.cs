using Data.Entities.Enums;
using Data.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configs.UserConfig
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUsers");
            builder.Property(x => x.FirstName).IsRequired(false).HasMaxLength(100);
            builder.Property(x => x.LastName).IsRequired(false).HasMaxLength(100);
            builder.Property(x => x.Address).IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.Gender).HasDefaultValue(Gender.Other);
            builder.Property(x => x.RefreshToken).IsRequired(false);
            builder.Property(x => x.UserLinks).IsRequired(false);
            builder.Property(x => x.Description).IsRequired(false);
            builder.Property(x => x.UserName).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.ImageUrl).IsRequired(false);
        }
    }
}
