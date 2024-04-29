using Microsoft.EntityFrameworkCore;
using AcmeCorpApi.Models;

namespace AcmeCorpApi.Repository
{
    public class OrdersDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public OrdersDbContext (DbContextOptions<OrdersDbContext> options) : base(options) { }
    }
}