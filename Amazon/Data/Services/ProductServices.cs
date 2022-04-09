using Amazon.Data.Base;
using Amazon.Models;

namespace Amazon.Data.Services
{
    public class ProductServices : EntityBaseRepository<Product>, IProductServices
    {
        AmazonDbContext _context;
        public ProductServices(AmazonDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAllCategoris()
        {
            return _context.categories.ToList();
        }

        public IEnumerable<Vendor> GetAllVendors()
        {
            return _context.Vendor.ToList();
        }
    }
}