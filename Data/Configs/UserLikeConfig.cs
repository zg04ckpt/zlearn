using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configs
{
    public class UserLikeConfig : IEntityTypeConfiguration<UserLike>
    {
        public void Configure(EntityTypeBuilder<UserLike> builder)
        {
            builder.ToTable("UserLikes");
            builder.HasKey(x => new { x.LikedUserId, x.UserId });
        }
    }
}
