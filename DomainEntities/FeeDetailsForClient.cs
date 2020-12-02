using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class FeeDetailsForClient
    {
        public string Type { get; set; }
        public string Month { get; set; }
        public decimal Fee { get; set; }
        public decimal Discount { get; set; }
    }

    public class CollectionDetailsForlient
    {
        public int ID { get; set; }
        public string FeeType { get; set; }
        public string Month { get; set; }
        public decimal Fee { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalFee { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal previousDue { get; set; }
        public decimal GrandTotal { get; set; }

        public decimal TotalPaid { get; set; }
        public decimal PaymentDue { get; set; }

        public DateTime PaymentDate { get; set; }
        public string PaymentMiti { get; set; }

    }
}
