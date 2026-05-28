using System;
using System.Collections.Generic;

namespace CyberSecuAwarenessBotWPF.Services
{
    public class ResponseService
    {
        private readonly Random _random = new Random();

        public string GetRandom(List<string> responses)
        {
            if (responses == null || responses.Count == 0)
                return "I don't have a response for that.";
            return responses[_random.Next(responses.Count)];
        }
    }
}