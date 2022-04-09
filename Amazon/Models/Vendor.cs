using Amazon.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace Amazon.Models
{
    public class Vendor: IEntityBase
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Location { get; set; }

        public string Phone { get; set; }
    }
}
