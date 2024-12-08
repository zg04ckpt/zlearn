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
    public class PromotionOrderConfiguration : IEntityTypeConfiguration<PromotionOrder>
    {
        public void Configure(EntityTypeBuilder<PromotionOrder> builder)
        {
            builder.ToTable("PromotionOrders");
            builder.HasKey(x => new { x.OrderId, x.PromotionId });

            builder.HasOne(x => x.Order)
                .WithMany(x => x.PromotionOrders)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Promotion)
                .WithMany(x => x.PromotionOrders)
                .HasForeignKey(x => x.PromotionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
