# CyberGuard Features

## GUI Design
- Dark cybersecurity-themed interface
- Sidebar with user profile and quick topics
- Chat bubbles for bot and user messages
- ASCII art logo display

## Keyword Recognition
- Recognises 10+ cybersecurity keywords
- password, phishing, scam, privacy, malware
- wifi, 2fa, vpn, backup, social engineering

## Sentiment Detection
- Detects: worried, curious, frustrated, positive
- Automatically responds with relevant tip
- Updates mood indicator in sidebar

## Memory & Recall
- Remembers user name
- Remembers user's favourite topic
- Personalises responses based on memory

## Random Responses
- Multiple responses per topic
- Randomly selected to keep conversation fresh
- Uses ResponseSelector delegate

## Conversation Flow
- Handles follow-up questions
- Keywords: "more", "another tip", "explain"
- Continues current topic seamlessly

## Voice
- Text-to-speech voice greeting on startup
- Toggle voice on/off with button
- Cleans emojis before speaking

## Error Handling
- 3 different unknown input responses
- Never crashes on unexpected input
- Graceful default responses
