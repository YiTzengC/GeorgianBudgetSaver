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
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime BoughtDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public Decimal Price { get; set; }
        [Required]
        public bool InStock { get; set; }
        public string Photo { get; set; }

        [Display(Name = "CourseProgram")]
        public int CourseProgramId { get; set; }
        public CourseProgram CourseProgram { set; get; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
