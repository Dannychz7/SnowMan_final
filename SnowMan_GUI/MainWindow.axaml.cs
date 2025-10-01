// File: SnowMan_GUI/MainWindow.axaml.cs
// Author: Daniel Chavez
// Date: 2025.09.30
// Editor: Anthony Petrosino
// Date: 2025.9.30
// Description:
// - MainWindow implements the graphical interface for the Snowman game using Avalonia.
// - Initializes the game, draws the background, snowflakes, and snowman parts based on wrong guesses.
// - Handles user input via a text box, ONLY validates single letter guesses and updating the game state.
// - Updates the displayed word, guessed letters, and messages based on player actions.
// - Provides buttons for guessing, starting a new game, and enabling/disabling input when game is over / starting.
// - Uses drawing methods to visually build the snowman incrementally and add snowflake effects.
// - Connects to SnowmanGame class to connect the core logic in a fully interactive GUI.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;

namespace SnowMan_GUI
{
    public partial class MainWindow : Window
    {
        private SnowmanGame game = null!;

        public MainWindow()
        {
            InitializeComponent();
            SnowmanCanvas.Background = Brushes.LightSkyBlue;
            StartNewGame();
        }

        private void StartNewGame()
        {
            game = new SnowmanGame();
            WordText.Text = game.GetDisplayWord();
            MessageText.Text = "";
            InputBox.Text = "";
            EnableInput();
            SnowmanCanvas.Children.Clear();
            DrawSnowman(game.WrongGuesses); // draw initial snowman, nothing at start
            DrawSnowflakes();
        }

        private void GuessButton_Click(object? sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(InputBox.Text)) return;

            if (InputBox.Text.Length != 1)
            {
                MessageText.Text = "Please enter a single letter, '-' or '''.";
                InputBox.Text = "";
                return;
            }

            char guess = InputBox.Text[0];
            string message = game.MakeGuess(guess);
            WordText.Text = game.GetDisplayWord();
            MessageText.Text = message;
            GuessedLettersText.Text = "Guessed: " + game.GetGuessedLetters();
            InputBox.Text = "";

            DrawSnowman(game.WrongGuesses); // update snowman after guess
            DrawSnowflakes();

            if (game.IsGameWon())
            {
                MessageText.Text += "\n You won!";
                DisableInput();
            }
            else if (game.IsGameOver())
            {
                MessageText.Text += $"\n Game over! Word Was: {game.CurrentWord}";
                DisableInput();
            }
        }

        private void NewGameButton_Click(object? sender, RoutedEventArgs e)
        {
            StartNewGame();
            GuessedLettersText.Text = "Guessed: ";
        }
        private void DisableInput()
        {
            InputBox.IsEnabled = false;
            GuessButton.IsEnabled = false;
        }

        private void EnableInput()
        {
            InputBox.IsEnabled = true;
            GuessButton.IsEnabled = true;
        }

        private void UpdateGuessedLetters()
        {
            GuessedLettersText.Text = "Guessed: " + string.Join(" ", game.GuessedLetters);
        }

        private void InputBox_KeyDown(object? sender, Avalonia.Input.KeyEventArgs enter)
        {
            if (enter.Key == Avalonia.Input.Key.Enter)
            {
                GuessButton_Click(sender, new RoutedEventArgs());
            }
        }

        private void DrawSnowflakes(int count = 30)
        {
            Random rnd = new Random();

            for (int i = 0; i < count; i++)
            {
                Ellipse snowflake = new Ellipse
                {
                    Width = 5,
                    Height = 5,
                    Fill = Brushes.White,
                    Opacity = rnd.NextDouble() * 0.8 + 0.2

                };
                Canvas.SetLeft(snowflake, rnd.Next(0, (int)SnowmanCanvas.Width - 15));
                Canvas.SetTop(snowflake, rnd.Next(0, (int)SnowmanCanvas.Height - 15));
                SnowmanCanvas.Children.Add(snowflake);
            }
        }

        // Draw snowman parts based on wrong guesses
        private void DrawSnowman(int wrongGuesses)
        {
            SnowmanCanvas.Children.Clear();

            // 1st wrong guess: bottom/base
            if (wrongGuesses >= 1)
            {
                Ellipse baseCircle = new Ellipse
                {
                    Width = 80,
                    Height = 80,
                    Fill = Brushes.White,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                Canvas.SetLeft(baseCircle, 60);
                Canvas.SetTop(baseCircle, 200);
                SnowmanCanvas.Children.Add(baseCircle);
            }

            // 2nd wrong guess: middle
            if (wrongGuesses >= 2)
            {
                Ellipse middleCircle = new Ellipse
                {
                    Width = 60,
                    Height = 60,
                    Fill = Brushes.White,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                Canvas.SetLeft(middleCircle, 70);
                Canvas.SetTop(middleCircle, 140);
                SnowmanCanvas.Children.Add(middleCircle);
            }

            // 3rd wrong guess: head
            if (wrongGuesses >= 3)
            {
                Ellipse head = new Ellipse
                {
                    Width = 40,
                    Height = 40,
                    Fill = Brushes.White,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                Canvas.SetLeft(head, 80);
                Canvas.SetTop(head, 100);
                SnowmanCanvas.Children.Add(head);
            }

            // 4th wrong guess: left arm
            if (wrongGuesses >= 4)
            {
                Line leftArm = new Line
                {
                    StartPoint = new Avalonia.Point(70, 160),
                    EndPoint = new Avalonia.Point(30, 140),
                    Stroke = Brushes.Brown,
                    StrokeThickness = 4
                };
                SnowmanCanvas.Children.Add(leftArm);
            }

            // 5th wrong guess: right arm
            if (wrongGuesses >= 5)
            {
                Line rightArm = new Line
                {
                    StartPoint = new Avalonia.Point(130, 160),
                    EndPoint = new Avalonia.Point(170, 140),
                    Stroke = Brushes.Brown,
                    StrokeThickness = 4
                };
                SnowmanCanvas.Children.Add(rightArm);
            }

            // 6th wrong guess: hat
            if (wrongGuesses >= 6)
            {
                Rectangle hat = new Rectangle
                {
                    Width = 40,
                    Height = 20,
                    Fill = Brushes.Black
                };
                Canvas.SetLeft(hat, 80);
                Canvas.SetTop(hat, 85);
                SnowmanCanvas.Children.Add(hat);
            }
        }
    }
}
