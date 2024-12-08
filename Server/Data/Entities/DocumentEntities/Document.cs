using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities.CommonEntities;
using Data.Entities.SystemEntities;

namespace Data.Entities.DocumentEntities
{
    public class Document : Content
    {
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string FilePath { get; set; }
        public Guid? PaymentInfoId { get; set; }
        public int PurchaseCount { get; set; }

        //Rela
        public PaymentInfo? PaymentInfo { get; set; }
        public List<Order> Orders { get; set; }
    }
}
