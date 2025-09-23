// using System;
// using System.Collections.Generic;
// using System.Runtime.CompilerServices;

// namespace SnowMan.GameLogic
// {
//     public class SnowmanGame
//     {
//         private int wrongGuesses = 0;   // keeps track of bad guesses
//         private int maxGuesses = 6;     // max mistakes allowed

//         // Function that returns true if game should keep running
//         private bool gameTrue()
//         {
//             return wrongGuesses < maxGuesses;
//         }

//         private string buildWordDisplay(string word, Dictionary<char, bool> letters)
//         {
//             string display = "";
//             foreach (char c in word)
//             {
//                 // normalize to lowercase for dictionary lookup
//                 char lower = char.ToLower(c);
//                 if (letters.ContainsKey(lower) && !letters[lower])
//                 {
//                     display += c + " ";
//                 }
//                 else
//                 {
//                     display += "_ ";
//                 }
//             }
//             return display.Trim();
//         }
//         private string buildFailWord(int count)
//         {
//             // Simple Snowman stages (6 wrong guesses max)
//             string[] stages = new string[]
//             {
//                 null,               // 0 mistakes → nothing
//                 "  O  ",            // 1 mistake → head
//                 "  O  \n  |  ",     // 2 mistakes → head + body
//                 "  O  \n /|  ",     // 3 mistakes → add left arm
//                 "  O  \n /|\\",     // 4 mistakes → add right arm
//                 "  O  \n /|\\\n / ",// 5 mistakes → left leg
//                 "  O  \n /|\\\n / \\" // 6 mistakes → full body
//             };

//             if (count < 0 || count >= stages.Length)
//                 return null; // default if out of bounds

//             return stages[count];
//         }

//         private bool gameWon(string word, Dictionary<char, bool> letters)
//         {
//             foreach (char c in word)
//             {
//                 char lower = char.ToLower(c); // normalize to lowercase
//                 if (letters.ContainsKey(lower) && letters[lower])
//                 {
//                     // if any letter is still true (not guessed), game is not won
//                     return false;
//                 }
//             }
//             return true; // all letters guessed
//         }

//         public void Run()
//         {
//             Console.WriteLine("Hello, Snowman Game!");
//             // Initilize variables here
//             string input;
//             bool firstTry = true;
//             string filePath = "../dictionary.txt";
//             string[] words = null; // declare outside
//             Random rand = new Random();
//             char letter;

//             Dictionary<char, bool> letters = new Dictionary<char, bool>();
//             for (char c = 'a'; c <= 'z'; c++)
//             {
//                 letters.Add(c, true);
//                 // Console.WriteLine(letters[c]);
//             }
//             letters.Add('-', true);
//             letters.Add('\'', true);

//             try
//             {
//                 words = File.ReadAllLines(filePath);
//                 // Console.WriteLine("File contents (line by line):");
//                 foreach (string line in words)
//                 {
//                     // Console.WriteLine(line);
//                     continue;
//                 }
//             }
//             catch (FileNotFoundException)
//             {
//                 Console.WriteLine($"Error: The file '{filePath}' was not found.");
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine($"An error occurred: {ex.Message}");
//             }
//             int randomIndex = rand.Next(0, words.Length);
//             string randomWord = words[randomIndex];
//             Console.WriteLine("Last element: " + words[words.Length - 1]);
//             Console.WriteLine("First element: " + words[0]);
//             Console.WriteLine("Random element: " + randomWord);

//             string wordDisplay = buildWordDisplay(randomWord, letters);
//             Console.WriteLine("Word to guess: " + wordDisplay);

//             while (gameTrue())
//             {
//                 do
//                 {
//                     Console.Write("Enter a letter: ");
//                     input = Console.ReadLine()?.Trim();

//                     if (string.IsNullOrEmpty(input) || input.Length != 1)
//                     {
//                         if (!firstTry)
//                         {
//                             Console.WriteLine("Please enter a valid letter or '-' or '\''");
//                         }
//                         firstTry = false;
//                         continue;
//                     }

//                     // valid single character, so store it
//                     letter = input[0];
//                     break;

//                 } while (true);

//                 string inputLower = input.ToLower();

//                 if (letters.ContainsKey(inputLower[0]))
//                 {
//                     if (letters[inputLower[0]] == true)
//                     {
//                         Console.WriteLine($"You guessed: {inputLower[0]}");
//                         if (randomWord.Contains(inputLower[0]))
//                         {
//                             Console.WriteLine($"The word contains the letter: {inputLower[0]}");
//                             letters[inputLower[0]] = false;
//                         }
//                         else
//                         {
//                             Console.WriteLine($"The word does not contain the letter: {inputLower[0]}");
//                             wrongGuesses++; // count bad guesses
//                             letters[inputLower[0]] = false;
//                             string failDisplay = buildFailWord(wrongGuesses);
//                             if (failDisplay != null)
//                                 Console.WriteLine(failDisplay);
//                         }

//                         // Update the display
//                         wordDisplay = buildWordDisplay(randomWord, letters);
//                         Console.WriteLine("Word to guess: " + wordDisplay);

//                         // Check for win
//                         if (gameWon(randomWord, letters))
//                         {
//                             Console.WriteLine("Congratulations! You've guessed the word!");
//                             break; // exit the main guessing loop
//                         }
//                     }
//                     else
//                     {
//                         Console.WriteLine($"You already guessed: {inputLower[0]}");
//                         wordDisplay = buildWordDisplay(randomWord, letters);
//                         Console.WriteLine("Word to guess: " + wordDisplay);
//                     }
//                 }
//                 else
//                 {
//                     Console.WriteLine($"Invalid character: {inputLower[0]}");
//                 }
//             }
//             Console.WriteLine("Game over! You've used all 6 tries.");
//             Console.WriteLine($"The word was: {randomWord}");

//         }
//     }
// }


using System;
using System.Collections.Generic;
using System.IO;

namespace SnowMan.GameLogic
{
    public class SnowmanGame
    {
        private int wrongGuesses = 0;
        private int maxGuesses = 6;
        private Dictionary<char, bool> letters;
        private string currentWord;
        private WordManager wordManager;
        private DisplayManager displayManager;
        private InputValidator inputValidator;

        public SnowmanGame()
        {
            wordManager = new WordManager();
            displayManager = new DisplayManager();
            inputValidator = new InputValidator();
            InitializeLetters();
        }

        public void Run()
        {
            Console.WriteLine("Hello, Snowman Game!");
            
            if (!InitializeGame())
                return;

            PlayGame();
            EndGame();
        }

        private bool InitializeGame()
        {
            currentWord = wordManager.GetRandomWord();
            if (string.IsNullOrEmpty(currentWord))
            {
                Console.WriteLine("Failed to load word dictionary.");
                return false;
            }

            displayManager.ShowWordToGuess(currentWord, letters);
            return true;
        }

        private void PlayGame()
        {
            while (IsGameActive())
            {
                char guessedLetter = inputValidator.GetValidLetter();
                ProcessGuess(guessedLetter);

                displayManager.ShowWordToGuess(currentWord, letters);

                if (IsGameWon())
                {
                    Console.WriteLine("Congratulations! You've guessed the word!");
                    return;
                }
            }
        }

        private void ProcessGuess(char letter)
        {
            char lowerLetter = char.ToLower(letter);

            if (letters[lowerLetter] == false)
            {
                Console.WriteLine($"You already guessed: {lowerLetter}");
                return;
            }

            Console.WriteLine($"You guessed: {lowerLetter}");
            letters[lowerLetter] = false;

            if (currentWord.ToLower().Contains(lowerLetter))
            {
                Console.WriteLine($"The word contains the letter: {lowerLetter}");
            }
            else
            {
                Console.WriteLine($"The word does not contain the letter: {lowerLetter}");
                wrongGuesses++;
                displayManager.ShowSnowman(wrongGuesses);
            }
        }

        private void InitializeLetters()
        {
            letters = new Dictionary<char, bool>();
            for (char c = 'a'; c <= 'z'; c++)
            {
                letters.Add(c, true);
            }
            letters.Add('-', true);
            letters.Add('\'', true);
        }

        private bool IsGameActive()
        {
            return wrongGuesses < maxGuesses;
        }

        private bool IsGameWon()
        {
            foreach (char c in currentWord)
            {
                char lower = char.ToLower(c);
                if (letters.ContainsKey(lower) && letters[lower])
                {
                    return false;
                }
            }
            return true;
        }

        private void EndGame()
        {
            if (wrongGuesses >= maxGuesses)
            {
                Console.WriteLine("Game over! You've used all 6 tries.");
            }
            Console.WriteLine($"The word was: {currentWord}");
        }
    }

    public class WordManager
    {
        private string filePath = "../dictionary.txt";
        private string[] words;
        private Random rand = new Random();

        public string GetRandomWord()
        {
            if (!LoadWords())
                return null;

            int randomIndex = rand.Next(0, words.Length);
            string randomWord = words[randomIndex];
            
            Console.WriteLine("Last element: " + words[words.Length - 1]);
            Console.WriteLine("First element: " + words[0]);
            Console.WriteLine("Random element: " + randomWord);
            
            return randomWord;
        }

        private bool LoadWords()
        {
            try
            {
                words = File.ReadAllLines(filePath);
                return words.Length > 0;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Error: The file '{filePath}' was not found.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
    }

    public class DisplayManager
    {
        public void ShowWordToGuess(string word, Dictionary<char, bool> letters)
        {
            string display = BuildWordDisplay(word, letters);
            Console.WriteLine("Word to guess: " + display);
        }

        public void ShowSnowman(int wrongGuesses)
        {
            string snowmanDisplay = BuildSnowmanDisplay(wrongGuesses);
            if (snowmanDisplay != null)
                Console.WriteLine(snowmanDisplay);
        }

        private string BuildWordDisplay(string word, Dictionary<char, bool> letters)
        {
            string display = "";
            foreach (char c in word)
            {
                char lower = char.ToLower(c);
                if (letters.ContainsKey(lower) && !letters[lower])
                {
                    display += c + " ";
                }
                else
                {
                    display += "_ ";
                }
            }
            return display.Trim();
        }

        private string BuildSnowmanDisplay(int count)
        {
            string[] stages = new string[]
            {
                null,               // 0 mistakes → nothing
                "  O  ",            // 1 mistake → head
                "  O  \n  |  ",     // 2 mistakes → head + body
                "  O  \n /|  ",     // 3 mistakes → add left arm
                "  O  \n /|\\",     // 4 mistakes → add right arm
                "  O  \n /|\\\n / ",// 5 mistakes → left leg
                "  O  \n /|\\\n / \\" // 6 mistakes → full body
            };

            if (count < 0 || count >= stages.Length)
                return null;

            return stages[count];
        }
    }

    public class InputValidator
    {
        public char GetValidLetter()
        {
            string input;
            bool firstTry = true;

            while (true)
            {
                Console.Write("Enter a letter: ");
                input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input) || input.Length != 1)
                {
                    if (!firstTry)
                    {
                        Console.WriteLine("Please enter a valid letter or '-' or '\''");
                    }
                    firstTry = false;
                    continue;
                }

                char letter = input[0];
                char lowerLetter = char.ToLower(letter);

                if (IsValidCharacter(lowerLetter))
                {
                    return letter;
                }

                Console.WriteLine($"Invalid character: {lowerLetter}");
                firstTry = false;
            }
        }

        private bool IsValidCharacter(char c)
        {
            return (c >= 'a' && c <= 'z') || c == '-' || c == '\'';
        }
    }
}