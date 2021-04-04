using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgianBudgetSaver.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerId { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Province { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Phone { get; set; }
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public Decimal Total { get; set; }

       /* public int AccountId { get; set; }
        public Account Account { get; set; }*/
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
