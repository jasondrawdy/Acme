namespace AcmeCorpApi.Tests
{
    public class ProductTests : BaseIntegrationTest
    {
        private static IntegrationTestWebAppFactory _factory {get; set;}
        public ProductTests(IntegrationTestWebAppFactory factory)
            : base(factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PostProduct_WithInvalidApiKey_ReturnsProduct()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "invalid-key");
            var productData = new
            {
                id = 1111,
                name = "Elden Ring",
                sku = "123456789",
                price = 59.99M
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(productData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/products", content);

            // Assert
            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task PostProduct_WithValidApiKey_ReturnsProduct()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "test-key-changeme");
            var productData = new
            {
                id = 111,
                name = "Elden Ring",
                sku = "123456789",
                price = 59.99M
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(productData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/products", content);
            
            // Assert
            Assert.Equal(201, (int)response.StatusCode);
        }
        
        [Fact]
        public async Task GetProducts_WithInvalidApiKey_ReturnsUnauthorizedResponse()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "invalid-key");

            // Act
            var response = await client.GetAsync($"/api/products");

            // Assert
            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetProducts_WithValidApiKey_ReturnsAllProducts()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "test-key-changeme");

            // Act
            var response = await client.GetAsync($"/api/products");

            // Assert
            Assert.Equal(200, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetProduct_WithValidApiKey_ReturnsProduct_ById()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "test-key-changeme");
            int productId = 1;

            // Act
            var response = await client.GetAsync($"/api/products/{productId}");

            // Assert
            Assert.Equal(200, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetProduct_ThatDoesntExist_ReturnsProduct_ById()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "test-key-changeme");
            int productId = 9000;

            // Act
            var response = await client.GetAsync($"/api/products/{productId}");

            // Assert
            Assert.Equal(404, (int)response.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_WithInvalidApiKey_ReturnsUnauthorizedResponse()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "invalid-key");
            var productData = new
            {
                id = 1,
                name = "Crash Bandicoot",
                sku = "over-9000",
                price = 20.00M
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(productData), Encoding.UTF8, "application/json");
            var response = await client.PutAsync("/api/products/1", content);
            
            // Assert
            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_WithValidApiKey_ReturnsSuccessResponse()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "test-key-changeme");
            var productData = new
            {
                id = 1,
                name = "Crash Bandicoot",
                sku = "over-9000",
                price = 20.00M
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(productData), Encoding.UTF8, "application/json");
            var response = await client.PutAsync("/api/products/1", content);
            
            // Assert
            Assert.Equal(200, (int)response.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_ThatDoesntExist_ReturnsBadRequestResponse()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "test-key-changeme");
            var productData = new
            {
                id = 9999,
                name = "Counter-Strike 1.6",
                sku = "123456789halflifesource",
                price = 4.99M
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(productData), Encoding.UTF8, "application/json");
            var response = await client.PutAsync("/api/products/1", content);
            
            // Assert
            Assert.Equal(400, (int)response.StatusCode);
        }

        [Fact]
        public async Task DeleteProduct_WithInvalidApiKey_ReturnsUnauthorizedResponse()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "invalid-key");
            int productId = 9;

            // Act
            var response = await client.DeleteAsync($"/api/products/{productId}");

            // Assert
            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task DeleteProduct_WithValidApiKey_ReturnsSuccessResponse()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "test-key-changeme");
            int productId = 10;

            // Act
            var response = await client.DeleteAsync($"/api/products/{productId}");

            // Assert
            Assert.Equal(200, (int)response.StatusCode);
        }

        [Fact]
        public async Task DeleteProduct_ThatDoesntExist_ReturnsNotFoundResponse()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "test-key-changeme");
            int productId = 9000;

            // Act
            var response = await client.DeleteAsync($"/api/products/{productId}");

            // Assert
            Assert.Equal(404, (int)response.StatusCode);
        }
    }
}