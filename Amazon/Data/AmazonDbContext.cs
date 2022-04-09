using Amazon.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Amazon.Data
{
    public class AmazonDbContext:IdentityDbContext<User>
    {
        public AmazonDbContext(DbContextOptions<AmazonDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();

        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    //modelBuilder.Entity<OrderItem>(builder=>
        //    //HasRequired(c => c.OderId).WithMany().WillCascadeOnDelete(false)
        //    //)

        //    //modelBuilder.Entity<ShoppingCartItem>().HasKey(am => new
        //    //{
        //    //    am.ProductId,
        //    //    am.UserId
        //    //});

        //    base.OnModelCreating(modelBuilder);
        //}

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        //Orders related tables
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        //public DbSet<ShoppingCartItem> ShoppingCart { get; set; }
        public DbSet<Amazon.Models.Vendor> Vendor { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
