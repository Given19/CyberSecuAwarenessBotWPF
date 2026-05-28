using System;
using System.Collections.Generic;
using System.Linq;
using CyberSecuAwarenessBotWPF.Models;

namespace CyberSecuAwarenessBotWPF.Services
{
    public delegate string ResponseSelector(string input);
    public delegate void SentimentHandler(string sentiment, string topic);

    public enum Sentiment { Neutral, Worried, Curious, Frustrated, Positive }

    public class ChatbotService
    {
        private readonly UserProfile _user = new UserProfile();
        private string _lastTopic = string.Empty;
        private string _lastResponse = string.Empty;
        private Sentiment _currentMood = Sentiment.Neutral;

        public event SentimentHandler OnSentimentDetected;

        private readonly Dictionary<string, List<string>> _keywordResponses =
            new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                ["password"] = new List<string>
            {
                "🔑 Use strong, unique passwords for every account — at least 12 characters mixing letters, numbers, and symbols.",
                "🔑 Never reuse passwords. A password manager like Bitwarden or 1Password can generate and store them safely.",
                "🔑 Enable two-factor authentication (2FA) alongside strong passwords for an extra security layer.",
                "🔑 Avoid dictionary words, your name, or birth dates in passwords. Attackers use automated tools that crack them instantly."
            },
                ["phishing"] = new List<string>
            {
                "🎣 Be suspicious of any email urging you to click a link immediately. Legitimate organisations never pressure you like that.",
                "🎣 Always verify the sender's email address — scammers spoof display names but the domain is usually wrong.",
                "🎣 Hover over links before clicking to preview the real URL. If it looks odd, don't click.",
                "🎣 Phishing sites often mimic real ones. Check the URL carefully — a single letter difference is a red flag.",
                "🎣 When in doubt, go directly to the organisation's official website instead of clicking any link."
            },
                ["scam"] = new List<string>
            {
                "⚠️ If an offer sounds too good to be true, it almost certainly is. Trust your instincts.",
                "⚠️ Romance scams are on the rise. Never send money to someone you haven't met in person.",
                "⚠️ Government agencies will NEVER call you demanding immediate payment or threatening arrest.",
                "⚠️ Tech-support scams: Microsoft and Apple will never call you unsolicited about your computer.",
                "⚠️ Verify charity requests independently before donating — scammers exploit disaster events."
            },
                ["privacy"] = new List<string>
            {
                "🔒 Review app permissions regularly — many apps request far more access than they need.",
                "🔒 Use a VPN on public Wi-Fi to encrypt your traffic and protect your data from eavesdroppers.",
                "🔒 Social media oversharing is risky. Keep birthdates, phone numbers, and addresses off public profiles.",
                "🔒 Enable private browsing and consider browser extensions like uBlock Origin to reduce tracking.",
                "🔒 Regularly audit which third-party apps have access to your Google, Facebook, or Apple accounts."
            },
                ["malware"] = new List<string>
            {
                "🦠 Keep your operating system and software updated — most malware exploits known, patched vulnerabilities.",
                "🦠 Only download software from official sources. Cracked software is a primary malware delivery vector.",
                "🦠 Use reputable antivirus software and keep it updated. Windows Defender is solid for everyday use.",
                "🦠 Ransomware encrypts your files and demands payment. Maintain offline backups so you're never held hostage."
            },
                ["wifi"] = new List<string>
            {
                "📶 Public Wi-Fi is unencrypted — never do banking or enter passwords on public networks without a VPN.",
                "📶 Change your home router's default admin username and password immediately after setup.",
                "📶 Use WPA3 encryption on your home router if available, or at minimum WPA2.",
                "📶 Disable Wi-Fi auto-connect on your devices to prevent connecting to evil twin hotspots."
            },
                ["2fa"] = new List<string>
            {
                "🔐 Two-factor authentication (2FA) adds a second verification step, making your accounts far harder to hijack.",
                "🔐 Use an authenticator app (like Google Authenticator or Authy) rather than SMS for stronger 2FA.",
                "🔐 Even if a hacker steals your password, 2FA stops them from logging in without your second factor."
            },
                ["vpn"] = new List<string>
            {
                "🌐 A VPN encrypts your internet traffic, hiding it from ISPs, hackers on public Wi-Fi, and some trackers.",
                "🌐 Choose a reputable VPN with a no-logs policy. Free VPNs often sell your data — defeating the purpose.",
                "🌐 VPNs are not total anonymity tools — your VPN provider can still see your traffic."
            },
                ["backup"] = new List<string>
            {
                "💾 Follow the 3-2-1 backup rule: 3 copies, 2 different media types, 1 offsite (e.g. cloud).",
                "💾 Test your backups regularly. An untested backup is not a real backup.",
                "💾 Automated cloud backups (OneDrive, Google Drive, Backblaze) ensure you never forget to back up."
            },
                ["social engineering"] = new List<string>
            {
                "🎭 Social engineering manipulates people rather than systems. Always verify requests for sensitive info.",
                "🎭 Pretexting involves an attacker creating a fake scenario to gain your trust. Stay sceptical.",
                "🎭 Tailgating (following someone through a secure door) is a physical social engineering attack. Challenge unknowns."
            }
            };

        private readonly Dictionary<string, Sentiment> _sentimentKeywords =
            new Dictionary<string, Sentiment>(StringComparer.OrdinalIgnoreCase)
            {
                ["worried"] = Sentiment.Worried,
                ["scared"] = Sentiment.Worried,
                ["anxious"] = Sentiment.Worried,
                ["afraid"] = Sentiment.Worried,
                ["nervous"] = Sentiment.Worried,
                ["curious"] = Sentiment.Curious,
                ["interested"] = Sentiment.Curious,
                ["wondering"] = Sentiment.Curious,
                ["want to know"] = Sentiment.Curious,
                ["frustrated"] = Sentiment.Frustrated,
                ["annoyed"] = Sentiment.Frustrated,
                ["confused"] = Sentiment.Frustrated,
                ["overwhelmed"] = Sentiment.Frustrated,
                ["angry"] = Sentiment.Frustrated,
                ["great"] = Sentiment.Positive,
                ["happy"] = Sentiment.Positive,
                ["excited"] = Sentiment.Positive,
                ["thanks"] = Sentiment.Positive,
                ["thank you"] = Sentiment.Positive,
            };

        private readonly Dictionary<Sentiment, string> _sentimentPreambles =
            new Dictionary<Sentiment, string>
            {
                [Sentiment.Worried] = "😟 It's completely understandable to feel that way — cybersecurity can feel overwhelming. Let me help put your mind at ease.\n\n",
                [Sentiment.Curious] = "🤔 Great curiosity! Wanting to learn is the first step to staying safe online. Here's what you should know:\n\n",
                [Sentiment.Frustrated] = "😤 I hear you — security can be frustrating. Let me break this down as simply as possible for you.\n\n",
                [Sentiment.Positive] = "😊 Love the energy! Here's something valuable to keep you on the right track:\n\n",
                [Sentiment.Neutral] = string.Empty
            };

        private readonly Dictionary<Sentiment, string> _sentimentDefaultTopics =
            new Dictionary<Sentiment, string>
            {
                [Sentiment.Worried] = "scam",
                [Sentiment.Curious] = "phishing",
                [Sentiment.Frustrated] = "password",
                [Sentiment.Positive] = "privacy",
                [Sentiment.Neutral] = string.Empty
            };

        private readonly List<string> _moreKeywords = new List<string>
        {
            "more", "another", "tell me more", "explain", "elaborate",
            "give me another", "continue", "go on", "again", "next tip"
        };

        private readonly Random _random = new Random();

        public string UserName => _user.Name;
        public string UserInterest => _user.Interest;
        public Sentiment CurrentMood => _currentMood;
        public IEnumerable<string> AvailableTopics => _keywordResponses.Keys;

        public string ProcessInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Please type something so I can help you! 😊";

            input = input.Trim();
            string lower = input.ToLower();

            Sentiment detectedSentiment = DetectSentiment(lower);

            if (IsGreeting(lower)) return BuildGreeting();

            string name = TryExtractName(lower, input);
            if (name != null)
            {
                _user.Name = name;
                return string.Format("👋 Nice to meet you, **{0}**! I'm CyberGuard, your personal cybersecurity assistant. What would you like to learn about today?", name);
            }

            string interest = TryExtractInterest(lower);
            if (interest != null)
            {
                _user.Interest = interest;
                return string.Format("⭐ Noted! I'll remember that you're particularly interested in **{0}**. It's a crucial part of staying safe online.\n\n", interest)
                       + BuildTopicResponse(interest, lower);
            }

            if (_moreKeywords.Any(k => lower.Contains(k)) && !string.IsNullOrEmpty(_lastTopic))
                return BuildTopicResponse(_lastTopic, lower);

            string matchedTopic = MatchKeyword(lower);
            if (matchedTopic != null)
            {
                _lastTopic = matchedTopic;
                return BuildSentimentPreamble() + BuildTopicResponse(matchedTopic, lower);
            }

            if (detectedSentiment != Sentiment.Neutral)
            {
                string fallbackTopic = _sentimentDefaultTopics[detectedSentiment];
                if (!string.IsNullOrEmpty(fallbackTopic))
                {
                    _lastTopic = fallbackTopic;
                    return BuildSentimentPreamble() + BuildTopicResponse(fallbackTopic, lower);
                }
            }

            if (lower.Contains("help") || lower.Contains("what can you do") || lower.Contains("topics"))
                return BuildHelpResponse();

            if (lower.Contains("how are you") || lower.Contains("how r u"))
                return "⚡ I'm running at full capacity and ready to help you stay safe online!";

            if (lower.Contains("remember") || lower.Contains("what do you know about me"))
                return BuildMemoryRecall();

            if (IsGoodbye(lower)) return BuildGoodbye();

            return BuildUnknownResponse();
        }

        private Sentiment DetectSentiment(string lower)
        {
            Sentiment detected = Sentiment.Neutral;
            foreach (var kvp in _sentimentKeywords)
                if (lower.Contains(kvp.Key)) { detected = kvp.Value; break; }

            if (detected != _currentMood)
            {
                _currentMood = detected;
                _user.Mood = detected.ToString();
                string topic = MatchKeyword(lower);
                if (OnSentimentDetected != null)
                    OnSentimentDetected(detected.ToString(), topic ?? string.Empty);
            }
            return detected;
        }

        private string BuildSentimentPreamble()
        {
            string preamble = _sentimentPreambles[_currentMood];
            return string.IsNullOrEmpty(preamble) ? string.Empty : preamble;
        }

        private string BuildTopicResponse(string topic, string input)
        {
            ResponseSelector selector = delegate (string s)
            {
                List<string> list;
                if (_keywordResponses.TryGetValue(topic, out list))
                    return list[_random.Next(list.Count)];
                return string.Empty;
            };

            string tip = selector(input);
            if (string.IsNullOrEmpty(tip)) return BuildUnknownResponse();

            _lastResponse = tip;
            return tip + BuildPersonalisedSuffix(topic);
        }

        private string BuildPersonalisedSuffix(string topic)
        {
            string result = "\n\n";
            if (!string.IsNullOrEmpty(_user.Interest) &&
                _user.Interest.Equals(topic, StringComparison.OrdinalIgnoreCase))
                result += string.Format("As someone interested in **{0}**, this is especially important for you, {1}! ", _user.Interest, _user.Name);
            result += "_Type 'more' or 'another tip' for additional advice, or ask me about a different topic._";
            return result;
        }

        private string MatchKeyword(string lower)
        {
            foreach (string key in _keywordResponses.Keys)
                if (lower.Contains(key)) return key;
            if (lower.Contains("hack") || lower.Contains("breach")) return "malware";
            if (lower.Contains("two factor") || lower.Contains("authenticat")) return "2fa";
            if (lower.Contains("network") || lower.Contains("wi-fi") || lower.Contains("wireless")) return "wifi";
            return null;
        }

        private string TryExtractName(string lower, string original)
        {
            string[] patterns = { "my name is ", "i am ", "i'm ", "call me " };
            foreach (string p in patterns)
            {
                int idx = lower.IndexOf(p);
                if (idx >= 0)
                {
                    string after = original.Substring(idx + p.Length).Trim().Split(' ')[0];
                    after = after.Trim('.', '!', ',', '?');
                    if (after.Length > 1) return char.ToUpper(after[0]) + after.Substring(1);
                }
            }
            return null;
        }

        private string TryExtractInterest(string lower)
        {
            string[] patterns = { "interested in ", "care about ", "worried about ", "want to learn about " };
            foreach (string p in patterns)
            {
                int idx = lower.IndexOf(p);
                if (idx >= 0)
                {
                    string after = lower.Substring(idx + p.Length).Trim().Split('.')[0].Trim();
                    string matched = MatchKeyword(after);
                    return matched ?? (after.Length > 2 ? after : null);
                }
            }
            return null;
        }

        private static bool IsGreeting(string lower)
        {
            return new[] { "hello", "hi", "hey", "good morning", "good afternoon", "good evening", "greetings", "howdy" }
                .Any(g => lower.Contains(g));
        }

        private static bool IsGoodbye(string lower)
        {
            return new[] { "bye", "goodbye", "exit", "quit", "see you", "later", "farewell" }
                .Any(b => lower.Contains(b));
        }

        private string BuildGreeting()
        {
            string nameStr = _user.Name == "friend" ? string.Empty : ", " + _user.Name;
            return string.Format("👋 Hello{0}! I'm **CyberGuard**, your cybersecurity awareness assistant.\n\n" +
                   "I can help you with:\n• 🔑 Passwords & authentication\n• 🎣 Phishing & email scams\n" +
                   "• 🔒 Online privacy\n• ⚠️ Scam detection\n• 🦠 Malware & viruses\n• 📶 Wi-Fi security\n\n" +
                   "What would you like to know? Or tell me your name to get started!", nameStr);
        }

        private string BuildHelpResponse()
        {
            return "🛡 **CyberGuard can help you with these topics:**\n\n" +
                string.Join("\n", _keywordResponses.Keys.Select(k => "• " + k)) +
                "\n\n_Just type a topic or ask a question naturally._";
        }

        private string BuildMemoryRecall()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("🧠 Here's what I remember about you:\n\n");
            sb.AppendLine("• **Name:** " + _user.Name);
            if (!string.IsNullOrEmpty(_user.Interest))
                sb.AppendLine("• **Main interest:** " + _user.Interest);
            sb.AppendLine("• **Current mood:** " + _user.Mood);
            return sb.ToString().Trim();
        }

        private string BuildGoodbye()
        {
            string name = _user.Name == "friend" ? string.Empty : ", " + _user.Name;
            return string.Format("👋 Stay safe online{0}! Remember — **you** are the first line of defence against cyber threats. See you next time! 🛡", name);
        }

        private string BuildUnknownResponse()
        {
            string[] responses =
            {
                "🤔 I'm not sure I understand. Could you try rephrasing? You can ask about passwords, phishing, privacy, scams, malware, or Wi-Fi security.",
                "💬 I didn't quite catch that. Try asking something like 'Tell me about phishing' or 'Give me a password tip'.",
                "🛡 Hmm, that's outside my expertise. I specialise in cybersecurity topics — passwords, scams, privacy, and more."
            };
            return responses[_random.Next(responses.Length)];
        }
    }
}