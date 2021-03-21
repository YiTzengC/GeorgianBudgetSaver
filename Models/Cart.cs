using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgianBudgetSaver.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
       /* public int AccountId { get; set; }
        public Account Account { get; set; }*/
    }
}
