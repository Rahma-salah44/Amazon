using Amazon.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amazon.Models
{
    public class Product:IEntityBase
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(30)]
        [Required]
        public string Name { get; set; }
        [Required]
        public string ImageURL { get; set; }
        [Required]
        [MaxLength(400)]
        public string Description { get; set; }
        [Required]
        public double PricePerUnit { get; set; }
   
        [ForeignKey("Category")]
        public int CategoryId { get; set;}

        [ForeignKey("Vendor")]
        public int VendorId { get; set; }

        //[ForeignKey("User")]
        //public string UserId { get; set;}    
        //public virtual User User { get; set; }     
        public virtual Category Category { get; set; }
        public virtual Vendor Vendor { get; set; }



    }
}
