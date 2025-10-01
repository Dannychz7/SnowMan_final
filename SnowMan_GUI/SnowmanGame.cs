using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SnowMan_GUI
{
    public class SnowmanGame
    {
        public int wrongGuesses;
        private int maxGuesses = 6;
        private Dictionary<char, bool> letters = new Dictionary<char, bool>();
        public int WrongGuesses => wrongGuesses;

        public string CurrentWord = string.Empty;

        // Track guessed letters
        public List<char> GuessedLetters { get; private set; } = new List<char>();

        public SnowmanGame()
        {
            wrongGuesses = 0;
            InitializeLetters();
            LoadAndPickWord();
            GuessedLetters.Clear();
        }

        private void InitializeLetters()
        {
            letters = new Dictionary<char, bool>();

            // Build the full set of valid characters
            List<char> validChars = new List<char>();
            for (char c = 'a'; c <= 'z'; c++)
                validChars.Add(c);

            validChars.Add('-');
            validChars.Add('\'');

            // Sort alphanumerically
            validChars.Sort();

            // Initialize dictionary
            foreach (char c in validChars)
                letters[c] = true;
        }

        public string GetGuessedLetters()
        {
            var guessed = new List<char>();

            foreach (var kvp in letters)
            {
                if (!kvp.Value) // false means guessed
                    guessed.Add(kvp.Key);
            }

            guessed.Sort(); // ensure alphanumeric order
            return string.Join(" ", guessed);
        }

        private void LoadAndPickWord()
        {
            if (File.Exists("dictionary.txt"))
            {
                CurrentWord = PickRandomWordFromFile("dictionary.txt");
            }
            else
            {
                string[] fallback = { "raid", "zeus", "abacus", "logos" };
                Random rand = new Random();
                CurrentWord = fallback[rand.Next(fallback.Length)];
            }
        }

        // This approach is slightly inefficient because the file is read twice, but .NET’s file streaming is efficient enough that the 
        // performance hit is negligible compared to having to hold a large array in mem which is only ever used once.
        private string PickRandomWordFromFile(string path)
        {
            var rand = new Random();

            // First pass: count total lines
            int lineCount = File.ReadLines(path).Count();

            // Pick random line number
            int target = rand.Next(lineCount);

            // Second pass: skip until the chosen line
            return File.ReadLines(path).Skip(target).First();
        }

        public string GetDisplayWord()
        {
            string display = "";
            foreach (char c in CurrentWord)
            {
                char lower = char.ToLower(c);
                display += (letters.ContainsKey(lower) && !letters[lower]) ? c + " " : "_ ";
            }
            return display.Trim();
        }

        public string MakeGuess(char letter)
        {
            letter = char.ToLower(letter);
            if (!letters.ContainsKey(letter)) return "Invalid character.";

            // Check if already guessed
            if (GuessedLetters.Contains(letter))
                return $"Already guessed '{letter}'.";

            // Record guess
            GuessedLetters.Add(letter);

            // Mark guessed
            letters[letter] = false;

            // Console.WriteLine($"Current Word: {CurrentWord}"); DEBUG line, uncomment if you want to see the chosen word
            if (CurrentWord.ToLower().Contains(letter))
                return $"Good guess! '{letter}' is in the word.";
            else
            {
                wrongGuesses++;
                return $"Wrong guess! '{letter}' is not in the word.";
            }
        }

        public bool IsGameWon()
        {
            foreach (char c in CurrentWord)
            {
                char lower = char.ToLower(c);
                if (letters.ContainsKey(lower) && letters[lower]) return false;
            }
            return true;
        }

        public bool IsGameOver() => wrongGuesses >= maxGuesses;

        public string BuildSnowmanDisplay()
        {
            string[] stages = new string[]
            {
                "",
                "  O  ",
                "  O  \n  |  ",
                "  O  \n /|  ",
                "  O  \n /|\\",
                "  O  \n /|\\\n / ",
                "  O  \n /|\\\n / \\"
            };
            return stages[wrongGuesses];
        }
    }
}
