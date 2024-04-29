namespace AcmeCorpApi.Tests
{
    public class GeneratorTests
    {
        [Fact]
        public void Generate_Integer_ReturnsUniqueNumber()
        {
            // Arrange
            var numberGenerator = new SecureRandom();

            // Act
            var result = numberGenerator.Next();

            // Assert
            Assert.True(result >= 0);
        }

        [Fact]
        public void Generate_Integer_WithMaxValue_ReturnsUniqueNumber()
        {
            // Arrange
            var numberGenerator = new SecureRandom();

            // Act
            var result = numberGenerator.Next(8192);

            // Assert
            Assert.True(result >= 0);
        }

        [Fact]
        public void Generate_Integer_InRange_ReturnsUniqueNumber()
        {
            // Arrange
            var numberGenerator = new SecureRandom();

            // Act
            var result = numberGenerator.Next(0, 8192);

            // Assert
            Assert.True(result >= 0);
        }

        [Fact]
        public void Generate_Decimal_ReturnsUniqueFloatingPointNumber()
        {
            // Arrange
            var numberGenerator = new SecureRandom();

            // Act
            var result = numberGenerator.NextDouble();

            // Assert
            Assert.True(result >= 0.0);
        }

        [Fact]
        public void Generate_Bytes_ReturnsArrayOfBytes()
        {
            // Arrange
            var numberGenerator = new SecureRandom();

            // Act
            var result = numberGenerator.GetBytes(8192);

            // Assert
            Assert.True((result is byte[]) && (result.Length > 0));
        }

        [Fact]
        public void Generate_UniqueString_With_RandomLength()
        {
            // Arrange
            var stringGenerator = new StringGenerator();
            var numberGenerator = new SecureRandom();
            int length = numberGenerator.Next(16, 8192);

            // Act
            var result = stringGenerator.GenerateString(length);

            // Assert
            Assert.True((result is string) && (result.Length == length));
        }

        [Fact]
        public void Generate_UniqueString_With_FixedLength()
        {
            // Arrange
            int length = 16;
            var generator = new StringGenerator();

            // Act
            var result = generator.GenerateString(length);

            // Assert
            Assert.True((result is string) && (result.Length == length));
        }

        [Fact]
        public void Generate_SeededString_With_FixedLength()
        {
            // Arrange
            int length = 16;
            int seed = 31337;
            var original = "7p6NZ2os2yNrRrAn";
            var generator = new StringGenerator();

            // Act
            var result = generator.GenerateString(length, seed);

            // Assert
            Assert.Equal(original, result);
        }

        [Fact]
        public void Generate_SeededString_With_FixedLength_WrongSeed()
        {
            // Arrange
            int length = 16;
            int seed = 00000001;
            var original = "7p6NZ2os2yNrRrAn";
            var generator = new StringGenerator();

            // Act
            var result = generator.GenerateString(length, seed);

            // Assert
            Assert.NotEqual(original, result);
        }
    }
}