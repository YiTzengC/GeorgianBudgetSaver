using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgianBudgetSaver.Models
{
    public class Order
    {
        public string OrderID { get; set; }
        public object AccountID { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        public Decimal Total { get; set; }


        public Account Account { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
