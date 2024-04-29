using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using AcmeCorpApi.Models;

namespace AcmeCorpApi.Repository
{
    public class ProductsDbSeeder
    {
        readonly ILogger _logger;

        public ProductsDbSeeder(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("ProductsDbSeederLogger");
        }

        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var productsDb = serviceScope.ServiceProvider.GetService<ProductsDbContext>();
                if (await productsDb.Database.EnsureCreatedAsync())
                {
                    if (!await productsDb.Products.AnyAsync()) 
                        await InsertProductsSampleData(productsDb);
                }
            }
        }

        public async Task InsertProductsSampleData(ProductsDbContext db)
        {
            var products = GetProducts();
            db.Products.AddRange(products);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception exp)
            {
                _logger.LogError($"Error in {nameof(ProductsDbSeeder)}: " + exp.Message);
                throw;
            }
        }

        private List<Product> GetProducts() 
        {
            var products = new List<Product> 
            {
                new Product { Id = 1, Name = "Red Acrylic Paint", Price = 69.99M, Sku = "abcd123"},
                new Product { Id = 2, Name = "iPhone 15", Price = 999.99M, Sku = "hello123"},
                new Product { Id = 3, Name = "Gatorade", Price = 4.99M, Sku = "cool123"},
                new Product { Id = 4, Name = "RTX 4090", Price = 1600.00M, Sku = "sweet123"},
                new Product { Id = 5, Name = "Basketball", Price = 14.99M, Sku = "anotherone123"},
                new Product { Id = 6, Name = "Helldivers 2", Price = 39.99M, Sku = "wedabest123"},
                new Product { Id = 7, Name = "Magic: The Gathering—Assassin's Creed Collector Booster Box", Price = 279.99M, Sku = "magic123"},
                new Product { Id = 8, Name = "Pokemon: Emerald", Price = 19.99M, Sku = "pokemon123"},
                new Product { Id = 9, Name = "Halo 2", Price = 29.99M, Sku = "halo123"},
                new Product { Id = 10, Name = "Gears of War", Price = 23.99M, Sku = "gearsofwar123"},
            };

            return products;
        }
    }
}