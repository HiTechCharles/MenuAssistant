# Menu Assistant

**Menu Assistant** - A Windows desktop application for creating and announcing weekly restaurant menus with text-to-speech capabilities.

[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8.1-blue.svg)](https://dotnet.microsoft.com/download/dotnet-framework)
[![Platform](https://img.shields.io/badge/platform-Windows-lightgrey.svg)](https://www.microsoft.com/windows)

## Overview

Menu Assistant is a Windows Forms application designed to help restaurant staff quickly create, read aloud, and print weekly menu specials. The application features built-in text-to-speech functionality to announce menu items, making it accessible and easy to use for kitchen staff and managers.

## Features

- 📝 **Weekly Menu Planning**: Create menus for Monday through Friday (including separate Friday breakfast and lunch)
- 🔊 **Text-to-Speech**: Built-in speech synthesis to read the menu aloud
- 🖨️ **Print Support**: Save and print multiple copies of the weekly menu
- 📅 **Automatic Date Range**: Automatically calculates and displays the upcoming Monday-Friday date range
- ⌨️ **Keyboard Shortcuts**: Accessible keyboard navigation between menu sections
- 💾 **Auto-Save**: Saves menu to `MenuAssistant.txt` in your Documents folder when printing

## System Requirements

- **Operating System**: Windows 7 or later
- **Framework**: .NET Framework 4.8.1
- **Audio**: Speakers or audio output device (for text-to-speech)
- **Printer**: Optional, for printing physical menus

## Installation

### Download Release
1. Download the latest release from the [Releases](../../releases) page
2. Extract the ZIP file to your preferred location
3. Run `TTMA.exe`

### Build from Source
1. Clone the repository:
   ```bash
   git clone https://github.com/HiTechCharles/MenuAssistant.git
   ```
2. Open `TTMA\TTMA.csproj` in Visual Studio 2019 or later
3. Restore NuGet packages (if any)
4. Build the solution (Ctrl+Shift+B)
5. Run the application (F5)

## Usage

### Creating a Menu

1. The application automatically displays the upcoming week's date range
2. Enter menu items for each day:
   - **Monday** - Main lunch menu
   - **Tuesday** - Main lunch menu
   - **Wednesday** - Main lunch menu
   - **Thursday** - Main lunch menu
   - **Friday Breakfast** - Morning meal
   - **Friday Lunch** - Afternoon meal
3. Use **Tab** to navigate between fields
4. Press **↑ (Up Arrow)** to have the current line read aloud

### Reading the Menu

Click the **Read** button (or use keyboard shortcut) to have the entire menu spoken aloud using text-to-speech.

### Printing the Menu

1. Select the number of copies using the numeric spinner
2. Click the **Print** button
3. The menu is automatically saved to `Documents\TTMA.txt` and sent to your default printer

### Getting Help

Click the **Help** button to hear instructions on how to use the application.

## Technical Details

### Technologies Used
- **Language**: C#
- **Framework**: .NET Framework 4.8.1
- **UI Framework**: Windows Forms
- **TTS Engine**: System.Speech.Synthesis
- **Printing**: System.Drawing.Printing

### Project Structure
```
MenuAssistant/
├── TTMA/
│   ├── Form1.cs              # Main form logic
│   ├── Form1.Designer.cs     # Form designer code
│   ├── Program.cs            # Application entry point
│   ├── Properties/           # Assembly and resource files
│   └── TTMA.csproj          # Project file
└── README.md
```

### Key Features Implementation
- **Automatic Date Calculation**: Calculates the next Monday-Friday range
- **Speech Synthesis**: Configurable rate (3) and volume (100)
- **Multi-Copy Printing**: Supports printing multiple copies in one operation
- **Error Handling**: Graceful error handling for file I/O and printing operations

## Configuration

The application uses the following default settings:
- **Speech Rate**: 3 (medium-fast)
- **Speech Volume**: 100 (maximum)
- **Font**: Tahoma, 16pt Bold (for printing)
- **Save Location**: `%USERPROFILE%\Documents\TTMA.txt`

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is open source. Please check the repository for license information.

## Author

**HiTechCharles**
- GitHub: [@HiTechCharles](https://github.com/HiTechCharles)

## Version

Current Version: 6.13.83.42

## Acknowledgments

- Built for "Totally Tiffany's Cafe"
- Uses Windows Speech API for text-to-speech functionality

## Support

If you encounter any issues or have questions, please [open an issue](../../issues) on GitHub.

---

Made with ❤️ for restaurant staff everywhere
