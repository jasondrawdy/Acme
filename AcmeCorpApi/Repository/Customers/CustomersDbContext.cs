using Microsoft.EntityFrameworkCore;
using AcmeCorpApi.Models;

namespace AcmeCorpApi.Repository
{
    public class CustomersDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<State> States { get; set; }

        public CustomersDbContext (DbContextOptions<CustomersDbContext> options) : base(options) { }
    }
}