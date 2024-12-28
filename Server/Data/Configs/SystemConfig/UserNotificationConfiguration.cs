using Data.Entities.SystemEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configs.SystemConfig
{
    public class UserNotificationConfiguration : IEntityTypeConfiguration<UserNotification>
    {
        public void Configure(EntityTypeBuilder<UserNotification> builder)
        {
            builder.ToTable("UserNotifications");
            builder.HasKey(x => new { x.UserId, x.NotificationId });
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.NotificationId);

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserNotifications)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Notification)
                .WithMany(x => x.UserNotifications)
                .HasForeignKey(x => x.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
