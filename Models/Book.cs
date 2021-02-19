using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgianBudgetSaver.Models
{
    public class Book
    {
        public string BookId { get; set; }
        public string UserId { get; set; }
        public string ProgramId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime BoughtDate { get; set; }
        public Decimal Price { get; set; }
        public bool InStock { get; set; }
    }
}
