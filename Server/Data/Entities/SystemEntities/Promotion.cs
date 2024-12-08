using Data.Entities.CommonEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.SystemEntities
{
    public class Promotion : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int DiscountRate { get; set; }
        public decimal DiscountAmount { get; set; }
        public string Currency { get; set; } // VNĐ
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActivated { get; set; }
        public int RemainingUses { get; set; }

        //Rela
        public List<PromotionOrder> PromotionOrders { get; set; }
    }
}
