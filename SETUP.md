========================================
     SnowMan Setup Checklist
   Cross-Platform Avalonia C# GUI App
========================================

Step 0 — Install .NET 9 SDK
----------------------------
Make sure you have the latest .NET 9 SDK installed.

macOS (Homebrew):
    brew uninstall --cask dotnet-sdk@8
    brew install --cask dotnet-sdk

Windows:
    - Uninstall any existing .NET 8 SDK from "Add/Remove Programs"
    - Download and install .NET 9 SDK from:
      https://dotnet.microsoft.com/en-us/download

Linux (Debian/Ubuntu-based):
    sudo apt remove dotnet-sdk-8.0
    sudo apt install dotnet-sdk-9.0

Check installation:
    dotnet --version
(Should return 9.x.x)


Step 1 — Create the Project (repo owner only)
---------------------------------------------
Only the person initializing the repo runs this.
Everyone else skips to Step 4 (Clone and Run).

dotnet new avalonia.app -o . --name SnowMan

If you get an error about missing templates:
    dotnet new install Avalonia.Templates

If you get an error about .NET 8:
    Upgrade to .NET 9 using the instructions in Step 0.


Step 2 — Add .gitignore (repo owner only)
-----------------------------------------
Create a .gitignore file in the project root with:

# --- C# / .NET build artifacts ---
bin/
obj/
out/
Debug/
Release/

# --- Visual Studio / VS Code ---
.vs/
*.user
*.suo
*.userprefs

# --- macOS ---
.DS_Store

# --- Linux ---
*~

# --- Windows ---
Thumbs.db
desktop.ini

# --- Logs and temp ---
*.log
*.tmp

# --- Rider / JetBrains ---
.idea/
*.iml


Step 3 — Test the App Locally
-------------------------------
dotnet run

✅ You should see a window with:
- Black background
- White "1" centered on the screen


Step 4 — How Each Teammate Runs It
-----------------------------------
git clone <YOUR_REPO_URL>
cd SnowMan
dotnet restore
dotnet run

 [x] Everyone should see the same black window with "1".

Notes
------
- Do NOT commit bin/, obj/, Debug/, or Release/ folders.
- Only source files (.cs, .axaml, .csproj, etc.) should be in git.
- If dotnet run fails, check your SDK:
      dotnet --version
  (Should be 9.x.x)

- Ensure you have avalonia.Templates installed, if not:
    dotnet new install Avalonia.Templates