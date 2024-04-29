namespace AcmeCorpApi.Tests
{
    public class CustomerTests : BaseIntegrationTest
    {
        private static IntegrationTestWebAppFactory _factory {get; set;}
        public CustomerTests(IntegrationTestWebAppFactory factory)
            : base(factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PostCustomer_WithInvalidApiKey_ReturnsCustomer()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "invalid-key");
            var customerData = new
            {
                id = 1111,
                firstName = "Alice",
                lastName = "Smith",
                email = "alice@example.com",
                homePhone = "123-456-7890",
                mobilePhone = "987-654-3210",
                address = "123 Main St",
                city = "Anytown",
                state = new { id = 0, name = "StateName", abbreviation = "SN" },
                zip = 12345,
                gender = "Female",
                orderCount = 0
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(customerData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/customers", content);

            // Assert
            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task PostCustomer_WithValidApiKey_ReturnsCustomer()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "test-key-changeme");
            var customerData = new
            {
                id = 111,
                firstName = "Alice",
                lastName = "Smith",
                email = "alice@example.com",
                homePhone = "123-456-7890",
                mobilePhone = "987-654-3210",
                address = "123 Main St",
                city = "Anytown",
                state = new { id = 0, name = "StateName", abbreviation = "SN" },
                zip = 12345,
                gender = "Female",
                orderCount = 0
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(customerData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/customers", content);
            
            // Assert
            Assert.Equal(201, (int)response.StatusCode);
        }
        
        [Fact]
        public async Task GetCustomers_WithInvalidApiKey_ReturnsUnauthorizedResponse()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "invalid-key");

            // Act
            var response = await client.GetAsync($"/api/customers");

            // Assert
            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetCustomers_WithValidApiKey_ReturnsAllCustomers()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "test-key-changeme");

            // Act
            var response = await client.GetAsync($"/api/customers");

            // Assert
            Assert.Equal(200, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetCustomer_WithValidApiKey_ReturnsCustomer_ById()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "test-key-changeme");
            int customerId = 1;

            // Act
            var response = await client.GetAsync($"/api/customers/{customerId}");

            // Assert
            Assert.Equal(200, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetCustomer_ThatDoesntExist_ReturnsCustomer_ById()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "test-key-changeme");
            int customerId = 9000;

            // Act
            var response = await client.GetAsync($"/api/customers/{customerId}");

            // Assert
            Assert.Equal(404, (int)response.StatusCode);
        }

        [Fact]
        public async Task UpdateCustomer_WithInvalidApiKey_ReturnsUnauthorizedResponse()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "invalid-key");
            var customerData = new
            {
                id = 1,
                firstName = "Alice",
                lastName = "Smith",
                email = "alice@example.com",
                homePhone = "123-456-7890",
                mobilePhone = "987-654-3210",
                address = "123 Main St",
                city = "Anytown",
                state = new { id = 0, name = "StateName", abbreviation = "SN" },
                zip = 12345,
                gender = "Female",
                orderCount = 0
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(customerData), Encoding.UTF8, "application/json");
            var response = await client.PutAsync("/api/customers/1", content);
            
            // Assert
            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task UpdateCustomer_WithValidApiKey_ReturnsSuccessResponse()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "test-key-changeme");
            var customerData = new
            {
                id = 1,
                firstName = "Alice",
                lastName = "Smith",
                email = "alice@example.com",
                homePhone = "123-456-7890",
                mobilePhone = "987-654-3210",
                address = "123 Main St",
                city = "Anytown",
                state = new { id = 0, name = "StateName", abbreviation = "SN" },
                zip = 12345,
                gender = "Female",
                orderCount = 0
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(customerData), Encoding.UTF8, "application/json");
            var response = await client.PutAsync("/api/customers/1", content);
            
            // Assert
            Assert.Equal(200, (int)response.StatusCode);
        }

        [Fact]
        public async Task UpdateCustomer_ThatDoesntExist_ReturnsBadRequestResponse()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "test-key-changeme");
            var customerData = new
            {
                id = 9999,
                firstName = "Alice",
                lastName = "Smith",
                email = "alice@example.com",
                homePhone = "123-456-7890",
                mobilePhone = "987-654-3210",
                address = "123 Main St",
                city = "Anytown",
                state = new { id = 0, name = "StateName", abbreviation = "SN" },
                zip = 12345,
                gender = "Female",
                orderCount = 0
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(customerData), Encoding.UTF8, "application/json");
            var response = await client.PutAsync("/api/customers/1", content);
            
            // Assert
            Assert.Equal(400, (int)response.StatusCode);
        }

        [Fact]
        public async Task DeleteCustomer_WithInvalidApiKey_ReturnsUnauthorizedResponse()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "invalid-key");
            int customerId = 9;

            // Act
            var response = await client.DeleteAsync($"/api/customers/{customerId}");

            // Assert
            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task DeleteCustomer_WithValidApiKey_ReturnsSuccessResponse()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "test-key-changeme");
            int customerId = 10;

            // Act
            var response = await client.DeleteAsync($"/api/customers/{customerId}");

            // Assert
            Assert.Equal(200, (int)response.StatusCode);
        }

        [Fact]
        public async Task DeleteCustomer_ThatDoesntExist_ReturnsNotFoundResponse()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("AcmeApiKey", "test-key-changeme");
            int customerId = 9000;

            // Act
            var response = await client.DeleteAsync($"/api/customers/{customerId}");

            // Assert
            Assert.Equal(404, (int)response.StatusCode);
        }
    }
}
