using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgianBudgetSaver.Models
{
    public class CourseProgram
    {
        public int CourseProgramId { get; set; }
        [Required]
        public string Title { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
