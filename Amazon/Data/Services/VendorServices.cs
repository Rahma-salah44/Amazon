using Amazon.Data.Base;
using Amazon.Models;

namespace Amazon.Data.Services
{
    public class VendorServices : EntityBaseRepository<Vendor>,IVendorServices
    {
        public VendorServices(AmazonDbContext context) : base(context) { }
    }
}


