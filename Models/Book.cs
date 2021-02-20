using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgianBudgetSaver.Models
{
    public class Book
    {
        public string BookID { get; set; }
        public string AccountID { get; set; }
        public string ProgramID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        [DataType(DataType.Date)]
        public DateTime BoughtDate { get; set; }
        public Decimal Price { get; set; }
        public bool InStock { get; set; }

        public Program Program { set; get; }
        public Account Account { set; get; }

        public List<Cart> Carts { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
