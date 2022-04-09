using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Amazon.Data.Enums;

namespace Amazon.Models
{
    public class User:IdentityUser
    {
        public User()
        {
            
            Orders = new HashSet<Order>();
            Products = new HashSet<Product>();
            ShoppingCarts = new HashSet<ShoppingCartItem>();
        }

        //public string Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        //public string Phone { get; set; }
        [MaxLength(60)]
        public string? Address { get; set; }
        [Required]
        public Enums.UserType UserType { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<ShoppingCartItem> ShoppingCarts { get; set; }
        
    }
}
