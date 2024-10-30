using System.Drawing;

namespace Challenge
{
    /// <summary>
    /// The WordFinder class is responsible for locating and counting the occurrences of specific words 
    /// in a character matrix. It uses a two-dimensional array to represent the matrix and a dictionary 
    /// to map each character to its positions within the matrix. The class supports searching for words 
    /// both horizontally and vertically, and it can return the top ten most frequently found words.
    /// </summary>
    public class WordFinder
    {
        private readonly char[,] _letterMatrix; // Matrix holding the characters
        private readonly Dictionary<char, List<Point>> _letterPositions; // Dictionary mapping each letter to its positions in the matrix
        private readonly int _numberOfRows; // Total number of rows in the matrix
        private readonly int _numberOfColumns; // Total number of columns in the matrix

        public WordFinder(IEnumerable<string> inputMatrix)
        {
            (_letterMatrix, _letterPositions) = BuildMatrixAndPositionMap(inputMatrix);
            _numberOfRows = _letterMatrix.GetLength(0);
            _numberOfColumns = _letterMatrix.GetLength(1);
        }

        // Method to find the top 10 words that exist in the matrix
        public IEnumerable<string> Find(IEnumerable<string> wordsToSearch)
        {
            var foundWordCount = new Dictionary<string, int>(); // Dictionary to hold word counts

            if (wordsToSearch == null || !wordsToSearch.Any())
            {
                return [];
            }

            foreach (var word in wordsToSearch.Distinct())
            {
                int occurrences = CountWordOccurrencesInMatrix(word);
                if (occurrences > 0)
                {
                    foundWordCount[word] = occurrences; // Store the count if the word exists
                }
            }

            // Return the top 10 words ordered by their occurrence count
            return foundWordCount
                .OrderByDescending(kvp => kvp.Value)
                .Take(10)
                .Select(kvp => kvp.Key);
        }

        // Method to count occurrences of a word in the matrix
        private int CountWordOccurrencesInMatrix(string word)
        {
            int count = 0;

            // Check if the word is not null, empty, or whitespace
            if (string.IsNullOrWhiteSpace(word))
            {
                return 0; // Return 0 if the word is invalid
            }

            // Get the first character of the word
            char firstCharacter = word[0];

            // Check if the first character exists in the letter positions dictionary
            if (!_letterPositions.TryGetValue(firstCharacter, out List<Point>? value))
            {
                return 0; // Return 0 if the first character is not found
            }

            // Retrieve all points on the board where the first character is located
            List<Point> startingPoints = value;

            // Check for the word in each possible starting point
            foreach (var startingPoint in startingPoints)
            {
                // Check in the row and column for the existence of the word
                if (CheckWordInRow(startingPoint, word))
                {
                    count++; // Increment count for row match
                }

                if (CheckWordInColumn(startingPoint, word))
                {
                    count++; // Increment count for column match
                }
            }
            return count; // Return the total count of occurrences found
        }

        // Method to check if the word exists in a specific row
        private bool CheckWordInRow(Point startingPoint, string word)
        {
            // Check if the word fits within the remaining columns
            if (_numberOfColumns - startingPoint.Y < word.Length)
            {
                return false; // Return false if word cannot fit
            }

            // Check each character in the row
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] != _letterMatrix[startingPoint.X, startingPoint.Y + i])
                {
                    return false; // Return false if characters do not match
                }
            }
            return true; // Return true if the entire word matches
        }

        // Method to check if the word exists in a specific column
        private bool CheckWordInColumn(Point startingPoint, string word)
        {
            // Check if the word fits within the remaining rows
            if (_numberOfRows - startingPoint.X < word.Length)
            {
                return false; // Return false if word cannot fit
            }

            // Check each character in the column
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] != _letterMatrix[startingPoint.X + i, startingPoint.Y])
                {
                    return false; // Return false if characters do not match
                }
            }
            return true; // Return true if the entire word matches
        }

        // Method to build the character matrix and the dictionary of letter positions
        private static (char[,], Dictionary<char, List<Point>>) BuildMatrixAndPositionMap(IEnumerable<string> input)
        {
            var rows = input.ToList();
            Dictionary<char, List<Point>> letterPositions = new();

            int rowCount = rows.Count; // Total number of rows
            int columnCount = rows[0].Length; // Total number of columns

            // Validate input constraints
            if (rowCount > 64 || columnCount > 64 || rows.Any(row => row.Length != columnCount))
            {
                throw new ArgumentException("Input must contain <= 64 strings, each string should be <=64 and of the same length.");
            }

            char[,] characterMatrix = new char[rowCount, columnCount];

            // Fill the matrix and populate the letter positions dictionary
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    char currentChar = rows[i][j]; // Current character in the matrix
                    characterMatrix[i, j] = currentChar; // Fill the character matrix

                    // Initialize the list of positions for the current character if it doesn't exist
                    if (!letterPositions.TryGetValue(currentChar, out List<Point>? value))
                    {
                        value = [];
                        letterPositions[currentChar] = value;
                    }

                    value.Add(new Point(i, j));
                }
            }
            return (characterMatrix, letterPositions); // Return the filled matrix and letter positions
        }
    }
}

