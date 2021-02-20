using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgianBudgetSaver.Models
{
    public class Account
    {
        public string AccountID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public List<Book> Books { get; set; }
        public List<Order> Orders { get; set; }
        public List<Cart> Carts { get; set; }
    }
}
