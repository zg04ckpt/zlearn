using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.SystemEntities
{
    public class PromotionOrder
    {
        public Guid OrderId { get; set; }
        public Guid PromotionId { get; set; }

        //Rela
        public Order Order { get; set; }
        public Promotion Promotion { get; set; }
    }
}
