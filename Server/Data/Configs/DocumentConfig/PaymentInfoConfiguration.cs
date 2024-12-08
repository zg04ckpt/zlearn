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
    public class PaymentInfoConfiguration : IEntityTypeConfiguration<PaymentInfo>
    {
        public void Configure(EntityTypeBuilder<PaymentInfo> builder)
        {
            builder.ToTable("PaymentInfos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AccountNumber).HasMaxLength(100);
            builder.Property(x => x.AccountName).HasMaxLength(100);
            builder.Property(x => x.BankName).HasMaxLength(100);
        }
    }
}
