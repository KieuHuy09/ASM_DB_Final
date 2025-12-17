using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM_DB_Final
{
    internal class Purchase
    {
        public string PurchaseID { get; set; }
        public string CustomerName { get; set; }
        public string ProductCode { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }
}
