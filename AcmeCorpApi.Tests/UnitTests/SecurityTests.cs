namespace AcmeCorpApi.Tests
{
    public class SecurityTests
    {
        private static Hashing _hashing {get; set;}
        private static Encryption _encryption {get; set;}
        public SecurityTests()
        {
            _hashing = new Hashing();
            _encryption = new Encryption();
        }

        [Fact]
        public void HashMessage_WithMD5_ReturnsHash()
        {
            // Arrange
            var message = "Hello, World!";
            var checksum = "65a8e27d8879283831b664bd8b7f0ad4";

            // Act
            var result = _hashing.ComputeMessageHash(message, Hashing.HashType.MD5);

            // Assert
            Assert.Equal(checksum, result);
        }

        [Fact]
        public void HashMessage_WithSHA1_ReturnsHash()
        {
            // Arrange
            var message = "Hello, World!";
            var checksum = "0a0a9f2a6772942557ab5355d76af442f8f65e01";

            // Act
            var result = _hashing.ComputeMessageHash(message, Hashing.HashType.SHA1);

            // Assert
            Assert.Equal(checksum, result);
        }

        [Fact]
        public void HashMessage_WithSHA256_ReturnsHash()
        {
            // Arrange
            var message = "Hello, World!";
            var checksum = "dffd6021bb2bd5b0af676290809ec3a53191dd81c7f70a4b28688a362182986f";

            // Act
            var result = _hashing.ComputeMessageHash(message, Hashing.HashType.SHA256);

            // Assert
            Assert.Equal(checksum, result);
        }

        [Fact]
        public void HashMessage_WithSHA384_ReturnsHash()
        {
            // Arrange
            var message = "Hello, World!";
            var checksum = "5485cc9b3365b4305dfb4e8337e0a598a574f8242bf17289e0dd6c20a3cd44a089de16ab4ab308f63e44b1170eb5f515";

            // Act
            var result = _hashing.ComputeMessageHash(message, Hashing.HashType.SHA384);

            // Assert
            Assert.Equal(checksum, result);
        }

        [Fact]
        public void HashMessage_WithSHA512_ReturnsHash()
        {
            // Arrange
            var message = "Hello, World!";
            var checksum = "374d794a95cdcfd8b35993185fef9ba368f160d8daf432d08ba9f1ed1e5abe6cc69291e0fa2fe0006a52570ef18c19def4e617c33ce52ef0a6e5fbe318cb0387";

            // Act
            var result = _hashing.ComputeMessageHash(message, Hashing.HashType.SHA512);

            // Assert
            Assert.Equal(checksum, result);
        }

        [Fact]
        public void EncryptMessage_WithAES_ReturnsEncodedString()
        {
            // Arrange
            var message = "Don't forget to drink your ovaltine!";
            var password = "itripledogdareya!";
            var ciphertext = "4aPizk3SP9cqNh0urtAudqjkxKUbjnGzfxWFc+JLyqJR35C48h+IJdy1k/sU4+oy";

            // Act
            var result = _encryption.EncryptText(message, password);
            
            // Assert
            Assert.Equal(ciphertext, result);
        }

        [Fact]
        public void EncryptMessage_WithAES_SaltedRandom_ReturnsEncodedString()
        {
            // Arrange
            var message = "...P6 chip with RISC architecture...";
            var password = "It’s in that place where I put that thing that time.";
            var ciphertext = "FnstDIYTSisQ0RYD1o5HJ8JYGMQ3MT5yaYfWjyM7IMXfXqCUVF6+gR7tr2GukmZz";

            // Act
            var result = _encryption.EncryptTextSalted(message, password);

            // Assert
            Assert.NotEqual(ciphertext, result);
        }

        [Fact]
        public void EncryptMessage_WithAES_SaltedFixed_ReturnsEncodedString()
        {
            // Arrange
            var message = "...P6 chip with RISC architecture...";
            var password = "It’s in that place where I put that thing that time.";
            var ciphertext = "FnstDIYTSisQ0RYD1o5HJ8JYGMQ3MT5yaYfWjyM7IMXfXqCUVF6+gR7tr2GukmZz";
            var salt = "zSJuXkwXuXU="; // Must be 8 bytes when decoded.

            // Act
            var result = _encryption.EncryptTextSalted(message, password, salt);

            // Assert
            Assert.Equal(ciphertext, result);
        }

        [Fact]
        public void DecryptMessage_WithAES_ReturnsPlaintextString()
        {
            // Arrange
            var message = "Don't forget to drink your ovaltine!";
            var password = "itripledogdareya!";
            var ciphertext = "4aPizk3SP9cqNh0urtAudqjkxKUbjnGzfxWFc+JLyqJR35C48h+IJdy1k/sU4+oy";

            // Act
            var result = _encryption.DecryptText(ciphertext, password);
            
            // Assert
            Assert.Equal(message, result);
        }

        [Fact]
        public void DecryptMessage_WithAES_WrongPassword_ReturnsPlaintextString()
        {
            // Arrange
            var message = "Don't forget to drink your ovaltine!";
            var password = "https://www.youtube.com/watch?v=or5C2jV1qRc";
            var ciphertext = "4aPizk3SP9cqNh0urtAudqjkxKUbjnGzfxWFc+JLyqJR35C48h+IJdy1k/sU4+oy";

            // Act
            var result = _encryption.DecryptText(ciphertext, password);
            
            // Assert
            Assert.NotEqual(message, result);
        }

        [Fact]
        public void DecryptMessage_WithAES_Salted_ReturnsPlaintextString()
        {
            // Arrange
            var message = "...P6 chip with RISC architecture...";
            var password = "It’s in that place where I put that thing that time.";
            var ciphertext = "FnstDIYTSisQ0RYD1o5HJ8JYGMQ3MT5yaYfWjyM7IMXfXqCUVF6+gR7tr2GukmZz";

            // Act
            var result = _encryption.DecryptTextSalted(ciphertext, password);

            // Assert
            Assert.Equal(message, result);
        }

        [Fact]
        public void DecryptMessage_WithAES_Salted_WrongPassword_ReturnsPlaintextString()
        {
            // Arrange
            var message = "...P6 chip with RISC architecture...";
            var password = "https://www.youtube.com/watch?v=xLijXXUabL8";
            var ciphertext = "FnstDIYTSisQ0RYD1o5HJ8JYGMQ3MT5yaYfWjyM7IMXfXqCUVF6+gR7tr2GukmZz";

            // Act
            var result = _encryption.DecryptTextSalted(ciphertext, password);

            // Assert
            Assert.NotEqual(message, result);
        }
    }
}