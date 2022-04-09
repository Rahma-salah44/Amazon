using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amazon.Models
{
    public class Order
    {
         
        [Key]
        public int Id { get; set; }
        public double TotalPrice { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
     
        public virtual User User { get; set; }

        public virtual List<OrderItem> OrderItems { get; set; }


    }
}
