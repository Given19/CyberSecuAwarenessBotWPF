using System.Collections.Generic;

namespace CyberSecuAwarenessBotWPF.Services
{
    public class SentimentService
    {
        private List<string> worriedWords;
        private List<string> frustratedWords;
        private List<string> curiousWords;

        public SentimentService()
        {
            worriedWords = new List<string>()
            {
                "worried",
                "scared",
                "afraid",
                "nervous",
                "unsafe"
            };

            frustratedWords = new List<string>()
            {
                "angry",
                "frustrated",
                "annoyed",
                "confused"
            };

            curiousWords = new List<string>()
            {
                "interested",
                "curious",
                "learn",
                "know more"
            };
        }

        public string DetectSentiment(string input)
        {
            input = input.ToLower();

            foreach (string word in worriedWords)
            {
                if (input.Contains(word))
                {
                    return "Worried";
                }
            }

            foreach (string word in frustratedWords)
            {
                if (input.Contains(word))
                {
                    return "Frustrated";
                }
            }

            foreach (string word in curiousWords)
            {
                if (input.Contains(word))
                {
                    return "Curious";
                }
            }

            return "Neutral";
        }

        public string GetSentimentResponse(string mood)
        {
            switch (mood)
            {
                case "Worried":
                    return "It is understandable to feel worried about cyber threats. Let me help you stay safe online.";

                case "Frustrated":
                    return "Cybersecurity can sometimes feel overwhelming, but I will explain it step-by-step.";

                case "Curious":
                    return "That is great! Learning about cybersecurity is very important.";

                default:
                    return "Let us continue learning about cybersecurity.";
            }
        }
    }
}