namespace AcmeCorpApi.Tests
{
    public class OrderTests : BaseIntegrationTest
    {
        private static IntegrationTestWebAppFactory _factory {get; set;}
        public OrderTests(IntegrationTestWebAppFactory factory): base(factory)
        {
            _factory = factory;
        }
        
        [Fact]
        public async Task PostOrder_WithInvalidApiKey_ReturnsOrder()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "invalid-key");
            var orderData = new
            {
                id = 1111,
                customerId = 1,
                productId = 1,
                shippingAddress = "12345 Testing Way",
                quantity = 1,
                price = 69.99M
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(orderData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/orders", content);

            // Assert
            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task PostOrder_WithValidApiKey_ReturnsOrder()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "test-key-changeme");
            var orderData = new
            {
                id = 111,
                customerId = 1,
                productId = 1,
                shippingAddress = "12345 Testing Way",
                quantity = 1,
                price = 69.99M
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(orderData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/orders", content);
            
            // Assert
            Assert.Equal(201, (int)response.StatusCode);
        }
        
        [Fact]
        public async Task GetOrders_WithInvalidApiKey_ReturnsUnauthorizedResponse()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "invalid-key");

            // Act
            var response = await client.GetAsync($"/api/orders");

            // Assert
            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetOrders_WithValidApiKey_ReturnsAllOrders()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "test-key-changeme");

            // Act
            var response = await client.GetAsync($"/api/orders");

            // Assert
            Assert.Equal(200, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetOrder_WithValidApiKey_ReturnsOrder_ById()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "test-key-changeme");
            int orderId = 1;

            // Act
            var response = await client.GetAsync($"/api/orders/{orderId}");

            // Assert
            Assert.Equal(200, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetOrder_ThatDoesntExist_ReturnsOrder_ById()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "test-key-changeme");
            int orderId = 9000;

            // Act
            var response = await client.GetAsync($"/api/orders/{orderId}");

            // Assert
            Assert.Equal(404, (int)response.StatusCode);
        }

        [Fact]
        public async Task UpdateOrder_WithInvalidApiKey_ReturnsUnauthorizedResponse()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "invalid-key");
            var orderData = new
            {
                id = 1,
                customerId = 1,
                productId = 1,
                shippingAddress = "12345 Testing Dr",
                quantity = 1,
                price = 999.99M
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(orderData), Encoding.UTF8, "application/json");
            var response = await client.PutAsync("/api/orders/1", content);
            
            // Assert
            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task UpdateOrder_WithValidApiKey_ReturnsSuccessResponse()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "test-key-changeme");
            var orderData = new
            {
                id = 1,
                customerId = 1,
                productId = 1,
                shippingAddress = "12345 Testing Dr",
                quantity = 1,
                price = 999.99M
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(orderData), Encoding.UTF8, "application/json");
            var response = await client.PutAsync("/api/orders/1", content);
            
            // Assert
            Assert.Equal(200, (int)response.StatusCode);
        }

        [Fact]
        public async Task UpdateOrder_ThatDoesntExist_ReturnsBadRequestResponse()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "test-key-changeme");
            var orderData = new
            {
                id = 9999,
                customerId = 1,
                productId = 1,
                shippingAddress = "12345 Testing Dr",
                quantity = 1,
                price = 999.99M
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(orderData), Encoding.UTF8, "application/json");
            var response = await client.PutAsync("/api/orders/1", content);
            
            // Assert
            Assert.Equal(400, (int)response.StatusCode);
        }

        [Fact]
        public async Task DeleteOrder_WithInvalidApiKey_ReturnsUnauthorizedResponse()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "invalid-key");
            int orderId = 9;

            // Act
            var response = await client.DeleteAsync($"/api/orders/{orderId}");

            // Assert
            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task DeleteOrder_WithValidApiKey_ReturnsSuccessResponse()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "test-key-changeme");
            int orderId = 10;

            // Act
            var response = await client.DeleteAsync($"/api/orders/{orderId}");

            // Assert
            Assert.Equal(200, (int)response.StatusCode);
        }

        [Fact]
        public async Task DeleteOrder_ThatDoesntExist_ReturnsNotFoundResponse()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "test-key-changeme");
            int orderId = 9000;

            // Act
            var response = await client.DeleteAsync($"/api/orders/{orderId}");

            // Assert
            Assert.Equal(404, (int)response.StatusCode);
        }
    }
}