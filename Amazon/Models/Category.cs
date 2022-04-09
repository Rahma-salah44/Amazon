using System.ComponentModel.DataAnnotations;

namespace Amazon.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(30)]
        [Required]
        public string Name { get; set; }       
       
    }
}
