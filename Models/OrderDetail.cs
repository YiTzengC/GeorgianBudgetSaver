using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgianBudgetSaver.Models
{
    public class OrderDetail
    {
        public string OrderDetialID { get; set; }
        public string OrderID { get; set; }
        public string BookID { get; set; }

        public Order Order { get; set; }
        public Book Book { get; set; }
    }
}
