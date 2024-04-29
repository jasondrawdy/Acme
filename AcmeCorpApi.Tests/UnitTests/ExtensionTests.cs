namespace AcmeCorpApi.Tests
{
    public class ExtensionTests
    {
        [Fact]
        public void Convert_Integer_ToBytes()
        {
            // Arrange
            int number = 31337;

            // Act
            byte[] result = number.ToBytes();

            // Assert
            Assert.True((result.GetType() != null) && (result.Length > 0));
        }

        [Fact]
        public void Convert_String_ToBytes()
        {
            // Arrange
            string data = "https://www.youtube.com/watch?v=2RKMtM2OebQ";

            // Act
            byte[] result = data.ToBytes();
            
            // Assert
            Assert.True((result.GetType() != null) && (result.Length > 0));
        }

        [Fact]
        public void GetString_From_ByteArray()
        {
            // Arrange
            string original = "https://www.youtube.com/watch?v=0qanF-91aJo";
            string encoded = "aHR0cHM6Ly93d3cueW91dHViZS5jb20vd2F0Y2g/dj0wcWFuRi05MWFKbw==";

            // Act
            byte[] result = Convert.FromBase64String(encoded);
            string decoded = result.GetString();

            // Assert
            Assert.Equal(original, decoded);
        }
    }
}