using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using AcmeCorpApi.Models;

namespace AcmeCorpApi.Repository
{
    public class OrdersDbSeeder
    {
        readonly ILogger _logger;

        public OrdersDbSeeder(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("OrdersDbSeederLogger");
        }

        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var ordersDb = serviceScope.ServiceProvider.GetService<OrdersDbContext>();
                if (await ordersDb.Database.EnsureCreatedAsync())
                {
                    if (!await ordersDb.Orders.AnyAsync()) 
                        await InsertOrdersSampleData(ordersDb);
                }
            }
        }

        public async Task InsertOrdersSampleData(OrdersDbContext db)
        {
            var orders = GetOrders();
            db.Orders.AddRange(orders);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception exp)
            {
                _logger.LogError($"Error in {nameof(OrdersDbSeeder)}: " + exp.Message);
                throw;
            }
        }

        private List<Order> GetOrders() 
        {
            var orders = new List<Order> 
            {
                new Order { CustomerId = 1, ProductId = 1, ShippingAddress = "123 Main Street, Apt 1, Cityville, State, Zip Code", Price = 69.99M, Quantity = 1 },
                new Order { CustomerId = 2, ProductId = 2, ShippingAddress = "456 Elm Avenue, Suite 202, Townsville, State, Zip Code", Price = 999.99M, Quantity = 2 },
                new Order { CustomerId = 3, ProductId = 3, ShippingAddress = "789 Oak Lane, Unit B, Villagetown, State, Zip Code", Price = 4.99M, Quantity = 4 },
                new Order { CustomerId = 4, ProductId = 4, ShippingAddress = "101 Pine Road, Building C, Hamlet City, State, Zip Code", Price = 1600.00M, Quantity = 5 },
                new Order { CustomerId = 5, ProductId = 5, ShippingAddress = "234 Maple Drive, Floor 2, Riverside, State, Zip Code", Price = 14.99M, Quantity = 1 },
                new Order { CustomerId = 6, ProductId = 6, ShippingAddress = "567 Cedar Street, Apartment 3D, Hillcrest Heights, State, Zip Code", Price = 39.99M, Quantity = 3 },
                new Order { CustomerId = 7, ProductId = 7, ShippingAddress = "890 Birch Court, Room 101, Valleyview, State, Zip Code", Price = 279.99M, Quantity = 9 },
                new Order { CustomerId = 8, ProductId = 8, ShippingAddress = "111 Willow Lane, House 5, Mountain Valley, State, Zip Code", Price = 19.99M, Quantity = 5 },
                new Order { CustomerId = 9, ProductId = 9, ShippingAddress = "222 Redwood Avenue, Suite 15, Lakeside Village, State, Zip Code", Price = 29.99M, Quantity = 1 },
                new Order { CustomerId = 10, ProductId = 10, ShippingAddress = "333 Spruce Street, Unit 20, Harborview, State, Zip Code", Price = 23.99M, Quantity = 1 }
            };

            return orders;
        }
    }
}