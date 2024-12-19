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
        public string FileName { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public int Size { get; set; } //B
        public Guid? PaymentInfoId { get; set; }
        public int PurchaseCount { get; set; }
        //Rela
        public PaymentInfo? PaymentInfo { get; set; }
        public List<Order> Orders { get; set; }
    }
}
