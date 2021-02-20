using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgianBudgetSaver.Models
{
    public class Cart
    {
        public string CartID { get; set; }
        public string BookID { get; set; }
        public string AccountID { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        public Book Book { get; set; }
        public Account Account { get; set; }
    }
}
