# Testing Guide

## How to Test the Chatbot

### Test 1 — Name Recognition
- Type: `my name is John`
- Expected: Bot greets you by name

### Test 2 — Keyword Recognition
- Type: `tell me about phishing`
- Expected: Bot responds with phishing tip

### Test 3 — Random Responses
- Type: `password` multiple times
- Expected: Different tips each time

### Test 4 — Sentiment Detection
- Type: `I am worried about scams`
- Expected: Bot detects worry and auto-replies with scam tip

### Test 5 — Memory Recall
- Type: `I am interested in privacy`
- Then type: `what do you know about me`
- Expected: Bot recalls your interest

### Test 6 — Conversation Flow
- Type any topic then type: `tell me more`
- Expected: Bot gives another tip on same topic

### Test 7 — Unknown Input
- Type: `blah blah blah`
- Expected: Bot responds with default unknown response

### Test 8 — Voice Toggle
- Click the Voice button
- Expected: Voice turns off/on

### Test 9 — Clear Chat
- Click the Clear button
- Expected: Chat clears and shows fresh message

### Test 10 — Goodbye
- Type: `bye`
- Expected: Bot says farewell with your name
