namespace AcmeCorpApi.Tests
{
    public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
    {
        private readonly IServiceScope _scope;
        protected readonly CustomersDbContext CustomersDbContext;
        protected readonly OrdersDbContext OrdersDbContext;
        protected readonly ProductsDbContext ProductsDbContext;

        protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
        {
            _scope = factory.Services.CreateScope();
            CustomersDbContext = _scope.ServiceProvider.GetRequiredService<CustomersDbContext>();
            OrdersDbContext = _scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
            ProductsDbContext = _scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
        }
    }
}
