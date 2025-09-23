***************************************************
Core Files

App.axaml
 - Defines global application resources and styles (similar to App.xaml in WPF).
 - You can declare theme colors, styles, or global resources that apply to the whole app.
 - Example: setting the application theme (FluentLight, FluentDark, etc.).

App.axaml.cs
 - The C# code-behind for App.axaml.
 - Contains the App class, which initializes your application (startup logic, lifecycle events, etc.).
 - Usually calls InitializeComponent() to load App.axaml.

MainWindow.axaml
 - Defines the layout of the main application window using XAML.
 - This is where you put UI elements like buttons, textboxes, grids, etc.

MainWindow.axaml.cs
 - Code-behind for MainWindow.axaml.
 - Contains event handlers and logic for the UI defined in the XAML file.
 - Example: what happens when a button is clicked.

Program.cs
 - Entry point of the app.
 - Defines the Main() method, which configures and starts Avalonia.
 - Typically creates and shows MainWindow when the application launches.

***************************************************
Project + Solution Files

SnowMan.csproj
 - The C# project file.
 - Lists project dependencies, target framework (like .NET 8), build settings, and package references (NuGet).

SnowMan_final.sln
 - The Visual Studio / Rider solution file.
 - Groups one or more projects (here, just SnowMan) into a solution so you can open everything together in an IDE.

***************************************************
Configuration + Metadata

app.manifest
 - Windows-specific application settings.
 - Defines things like compatibility, DPI awareness, and requested privileges.

=======================================================================================
Some Directions on How to do Snowman:

1. Program randomly selects a word from the dictionary.txt file (separated by "\n")
2. Player basiclly plays hangman
    - Player chooses any letter in the Alphabet (case insenitive, so do something like UserInput.lower())
    - NOTE: numbers and symbols should throw errors, or not valid input
    - NOTE: hypens and apostrophes are legal guesses
        - If right: All instances of that letter in that word are revealed
        - If wrong: Guesses are show alpha-numericly (basicly sorted) AND CANNOT be chosen again
3. Game ends when either:
    1. All letters in the word have been guessed
    2. Player has used 6 incorrect guesses (all parts of snowman have been drawn)
    3. Show player has won or lose