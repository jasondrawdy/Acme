using Microsoft.EntityFrameworkCore;
using AcmeCorpApi.Models;

namespace AcmeCorpApi.Repository
{
    public class ProductsDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ProductsDbContext (DbContextOptions<ProductsDbContext> options) : base(options) { }
    }
}