using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgianBudgetSaver.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        [DataType(DataType.Date)]
        public DateTime BoughtDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal Price { get; set; }
        public bool InStock { get; set; }

        [Display(Name = "CourseProgram")]
        public int CourseProgramId { get; set; }
        public CourseProgram CourseProgram { set; get; }
       /* [Display(Name = "Account")]
        public int AccountId { get; set; }*/
       /* public Account Account { set; get; }*/
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
