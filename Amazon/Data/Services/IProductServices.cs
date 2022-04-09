using Amazon.Data.Base;
using Amazon.Models;

namespace Amazon.Data.Services
{
    public interface IProductServices : IEntityBaseRepository<Product>
    {
        public IEnumerable<Category> GetAllCategoris();
        public IEnumerable<Vendor> GetAllVendors();
    }
}