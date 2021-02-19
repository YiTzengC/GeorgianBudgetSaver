using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgianBudgetSaver.Models
{
    public class Cart
    {
        public string CartId { get; set; }
        public string BookId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
