using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amazon.Models
{
    public class ShoppingCartItem
    {


        //[Key, Column(Order = 0)]
        //[ForeignKey("User")]
        //public string UserId { get; set; }
        //[Key, Column(Order = 1)]
        //[ForeignKey("Product")]
        //public int ProductId { get; set; }
        //public int Quantity { get; set; }

        //public virtual User User { get; set; }

        //public virtual Product Product { get; set; }

        [Key]
        public int Id { get; set; }

        public virtual Product product { get; set; }
        public int Amount { get; set; }

        public string ShoppingCartId { get; set; }
    }
}
