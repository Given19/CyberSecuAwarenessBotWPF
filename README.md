# CyberGuard - Cybersecurity Awareness Chatbot

![GitHub Actions](https://github.com/Given19/CyberSecuAwarenessBotWPF/workflows/Build%20Check/badge.svg)

A WPF-based Cybersecurity Awareness Chatbot built for South African citizens as part of PROG6221 Part 2 at The Independent Institute of Education.

---

## Description

CyberGuard is an interactive cybersecurity awareness assistant that educates users about online threats through a conversational dark-themed GUI. It recognises cybersecurity keywords, detects user sentiment, remembers user details, and provides varied responses to keep the conversation engaging.

---

## Features

### GUI Design
- Dark cybersecurity-themed interface
- Chat bubbles for bot and user messages
- Sidebar with user profile and quick topic buttons
- ASCII art logo displayed in sidebar
- Mood indicator that updates based on sentiment

### Keyword Recognition
Recognises 10+ cybersecurity keywords including password, phishing, scam, privacy, malware, wifi, 2fa, vpn, backup, and social engineering.

### Sentiment Detection
Detects and responds to worried, curious, frustrated, and positive sentiments. Automatically replies with a relevant tip without requiring the user to type again.

### Memory and Recall
- Remembers user name and favourite cybersecurity topic
- Personalises responses based on stored details
- Type "what do you know about me" to see stored memory

### Conversation Flow
Supports follow-up commands such as "more", "another tip", "tell me more", "explain", "elaborate", and "continue".

### Voice
- Text-to-speech greeting on startup using System.Speech
- Toggle voice on/off with the Voice button

---

## How to Run

### Requirements
- Windows OS
- Visual Studio 2022
- .NET Framework 4.7.2 or higher
- System.Speech (included in .NET Framework)

### Steps

1. Clone the repository:
git clone https://github.com/Given19/CyberSecuAwarenessBotWPF.git

2. Open CyberSecuAwarenessBotWPF.sln in Visual Studio

3. Build the solution using Ctrl + Shift + B

4. Run the application using F5

---

## How to Use

| Type This | What Happens |
|---|---|
| my name is [name] | Bot remembers your name |
| tell me about phishing | Gets phishing tips |
| I am worried about scams | Sentiment detected, auto-reply |
| more or another tip | Gets another tip on same topic |
| what do you know about me | Bot recalls your details |
| help | Shows all available topics |
| bye | Bot says farewell |

---

## Project Structure

CyberSecuAwarenessBotWPF/
- Models/
  - UserProfile.cs — Auto properties for user data
- Services/
  - ChatbotService.cs — Core chatbot logic
  - AudioPlayer.cs — Text-to-speech service
  - ResponseService.cs — Random response helper
- UI/
  - ConsoleUI.cs — UI helper methods
- ChatBubble.cs — Custom WPF chat bubble
- MainWindow.xaml — Main GUI layout
- MainWindow.xaml.cs — Main window logic
- App.xaml — Global styles
- App.xaml.cs — App entry point

---

## OOP Concepts Used

- Delegates — ResponseSelector and SentimentHandler
- Generic Collections — Dictionary with List of strings
- Encapsulation — Private fields with public properties
- Abstraction — Service classes hide implementation details

---

## Author

Ndivhuwo — PROG6221 Part 2 — 2026
The Independent Institute of Education

---

## License

MIT License
