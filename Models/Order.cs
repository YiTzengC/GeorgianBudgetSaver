using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgianBudgetSaver.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime OrderDate { get; set; }
        public Decimal Total { get; set; }
    }
}
