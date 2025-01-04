using Data.Entities.NotificationEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configs.NotificationConfig
{
    public class ReadNotificationConfiguration : IEntityTypeConfiguration<ReadNotification>
    {
        public void Configure(EntityTypeBuilder<ReadNotification> builder)
        {
            builder.ToTable("ReadNotifications");
            builder.HasKey(x => new {x.UserId, x.NotificationId});

            builder.HasOne(x => x.User)
                .WithMany(x => x.ReadNotifications)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Notification)
                .WithMany(x => x.ReadNotifications)
                .HasForeignKey(x => x.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
