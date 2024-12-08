using Data.Entities.CommonEntities;
using Data.Entities.DocumentEntities;
using Data.Entities.Enums;
using Data.Entities.UserEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.SystemEntities
{
    public class Order : BaseEntity
    {
        public int Quantity { get; set; }
        public Guid DocumentId { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; } // VNĐ
        public decimal DiscountAmount { get; set; }
        public decimal Total { get; set; }
        public OrderStatus Status { get; set; }
        public Guid UserId { get; set; }

        //Rela
        public Document Document { get; set; }
        public AppUser User { get; set; }
        public List<PromotionOrder> PromotionOrders { get; set; }
        public List<Payment> Payments { get; set; }
    }
}
