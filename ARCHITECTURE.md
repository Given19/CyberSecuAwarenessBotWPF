# Project Architecture

## Folder Structure
CyberSecuAwarenessBotWPF/
├── Models/
│   └── UserProfile.cs        # Stores user name, interest, mood
├── Services/
│   ├── ChatbotService.cs     # Core chatbot logic and responses
│   ├── AudioPlayer.cs        # Text-to-speech voice service
│   └── ResponseService.cs    # Random response helper
├── UI/
│   └── ConsoleUI.cs          # UI helper methods
├── ChatBubble.cs             # Chat message bubble control
├── MainWindow.xaml           # Main GUI layout
├── MainWindow.xaml.cs        # Main window logic
├── App.xaml                  # Application resources and styles
└── App.xaml.cs               # Application entry point

## Key Classes
- **ChatbotService** — processes user input and returns responses
- **UserProfile** — stores user memory using auto properties
- **AudioPlayer** — handles voice using System.Speech
- **ChatBubble** — custom WPF control for chat messages
- **ConsoleUI** — static helper for UI updates

## Delegates Used
- **ResponseSelector** — selects random re
