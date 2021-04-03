using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgianBudgetSaver.Models
{
    public class Cart
    {
        public int Quantity {get; set;}
        public int BookId { get; set; }
    }
}
