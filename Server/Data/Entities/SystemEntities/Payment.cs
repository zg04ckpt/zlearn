using Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.SystemEntities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public PaymentStatus Status { get; set; }
        public Guid OrderId { get; set; }
        public decimal amount { get; set; }
        public string Currency { get; set; } // VNĐ
        public string TransactionReference { get; set; }

        //Rela
        public Order Order { get; set; }
    }
}
