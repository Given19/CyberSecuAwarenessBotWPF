using System.Speech.Synthesis;

namespace CyberSecuAwarenessBotWPF.Services
{
    public class SpeechService
    {
        private SpeechSynthesizer speaker;

        public SpeechService()
        {
            speaker = new SpeechSynthesizer();
        }

        public void Speak(string text)
        {
            speaker.SpeakAsync(text);
        }
    }
}