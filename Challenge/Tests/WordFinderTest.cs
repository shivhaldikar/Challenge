using Xunit;

namespace Challenge.Tests
{
    public class WordFinderTests
    {
        [Fact]
        public void Constructor_ValidInput_CreatesMatrixAndPointer()
        {
            // Arrange
            var inputMatrix = new List<string>
            {
                "abc",
                "def",
                "ghi"
            };

            // Act
            var wordFinder = new WordFinder(inputMatrix);

            // Assert
            Assert.NotNull(wordFinder); // Ensure the object is created
        }

        [Theory]
        [InlineData("abc", 1)]
        [InlineData("def", 1)]
        [InlineData("ghi", 1)]
        [InlineData("abcd", 0)] // Word that doesn't exist
        public void CountWordOccurrencesInMatrix_ValidWords_ReturnsCorrectCount(string word, int expectedCount)
        {
            // Arrange
            var inputMatrix = new List<string>
            {
                "abc",
                "def",
                "ghi"
            };
            var wordFinder = new WordFinder(inputMatrix);

            // Act
            int count = wordFinder.Find(new List<string> { word }).Count();

            // Assert
            Assert.Equal(expectedCount, count);
        }

        [Fact]
        public void Find_TopWords_ReturnsTop10Words()
        {
            // Arrange
            var inputMatrix = new List<string>
            { 
                "abcdc", 
                "fgwio", 
                "chill", 
                "pqnsd", 
                "uvdxy" 
            };
            var wordFinder = new WordFinder(inputMatrix);
            var wordsToSearch = new List<string>
            {
                "chill", "cold", "wind", "Test", "old", "oldy"
            };

            // Act
            var topWords = wordFinder.Find(wordsToSearch).ToList();

            // Assert
            Assert.Contains("chill", topWords);
            Assert.Contains("cold", topWords);
            Assert.Contains("wind", topWords);
            Assert.Contains("old", topWords);
            Assert.Contains("oldy", topWords);
            Assert.Equal(5, topWords.Count); // Check that we only get existing words
        }

        [Fact]
        public void Find_NullOrWhitespaceWord_ReturnsEmpty()
        {
            // Arrange
            var inputMatrix = new List<string>
            {
                "abc",
                "def",
                "ghi"
            };
            var wordFinder = new WordFinder(inputMatrix);
            var wordsToSearch = new List<string> {"   ", "" };

            // Act
            var topWords = wordFinder.Find(wordsToSearch).ToList();

            // Assert
            Assert.Empty(topWords); // Should return no words
        }

        [Fact]
        public void Constructor_InvalidMatrix_ThrowsArgumentException()
        {
            // Arrange
            var inputMatrix = new List<string>
            {
                "abc",
                "de",
                "fg"
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new WordFinder(inputMatrix));
            Assert.Equal("Input must contain <= 64 strings, each string should be <=64 and of the same length.", exception.Message);
        }
    }
}
