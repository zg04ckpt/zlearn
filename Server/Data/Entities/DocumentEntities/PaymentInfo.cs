using Data.Entities.CommonEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.DocumentEntities
{
    public class PaymentInfo : BaseEntity
    {
        public decimal Price { get; set; }
        public string Currency { get; set; } // VNĐ
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string BankName { get; set; }

        //Rela
        public Document Document { get; set; }
    }
}
