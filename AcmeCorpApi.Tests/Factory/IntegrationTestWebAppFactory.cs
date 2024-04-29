namespace AcmeCorpApi.Tests
{
    public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        // TODO: Modularize and implement dynamic discovery later due to time constraints.
        private readonly PostgreSqlContainer _customersContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("customers")
            .WithUsername("postgres")
            .WithPassword("password")
            .Build();

        private readonly PostgreSqlContainer _ordersContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("orders")
            .WithUsername("postgres")
            .WithPassword("password")
            .Build();

        private readonly PostgreSqlContainer _productsContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("products")
            .WithUsername("postgres")
            .WithPassword("password")
            .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptorCustomers = services
                    .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<CustomersDbContext>));

                var descriptorOrders = services
                    .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<OrdersDbContext>));
                
                var descriptorProducts = services
                    .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ProductsDbContext>));

                if (descriptorCustomers is not null)
                    services.Remove(descriptorCustomers);

                if (descriptorOrders is not null)
                    services.Remove(descriptorOrders);

                if (descriptorProducts is not null)
                    services.Remove(descriptorProducts);

                services.AddDbContext<CustomersDbContext>(options => { options.UseNpgsql(_customersContainer.GetConnectionString()); });
                services.AddDbContext<OrdersDbContext>(options => { options.UseNpgsql(_ordersContainer.GetConnectionString()); });
                services.AddDbContext<ProductsDbContext>(options => { options.UseNpgsql(_productsContainer.GetConnectionString()); });
            });
        }

        public Task InitializeAsync()
        {
            _ = _customersContainer.StartAsync();
            _ = _ordersContainer.StartAsync();
            return _productsContainer.StartAsync();
        }

        public new Task DisposeAsync()
        {
            _ = _customersContainer.StopAsync();
            _ = _ordersContainer.StopAsync();
            return _productsContainer.StopAsync();
        }
    }
}