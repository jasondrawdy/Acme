namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<CustomersDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("CustomersConnectionString")));
            services.AddDbContext<OrdersDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("OrdersConnectionString")));
            services.AddDbContext<ProductsDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("ProductsConnectionString")));

            services.AddScoped<ICustomersRepository, CustomersRepository>();
            services.AddScoped<IOrdersRepository, OrdersRepository>();
            services.AddScoped<IProductsRepository, ProductsRepository>();

            return services;
        }
    }
}

