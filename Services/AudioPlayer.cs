using System;
using System.Speech.Synthesis;
using System.Threading.Tasks;

namespace CyberSecuAwarenessBotWPF.Services
{
    public class AudioPlayer : IDisposable
    {
        private readonly SpeechSynthesizer _synth;
        private bool _disposed;

        public bool IsEnabled { get; set; } = true;

        public AudioPlayer()
        {
            _synth = new SpeechSynthesizer();
            _synth.Volume = 85;
            _synth.Rate = 0;
            foreach (var voice in _synth.GetInstalledVoices())
                if (voice.VoiceInfo.Gender == VoiceGender.Female)
                {
                    _synth.SelectVoice(voice.VoiceInfo.Name);
                    break;
                }
        }

        public Task SpeakAsync(string text)
        {
            if (!IsEnabled || _disposed) return Task.FromResult(0);
            string clean = text
                .Replace("**", "").Replace("*", "").Replace("_", "")
                .Replace("🛡", "").Replace("🔑", "").Replace("🎣", "")
                .Replace("⚠️", "").Replace("🔒", "").Replace("🦠", "")
                .Replace("📶", "").Replace("🔐", "").Replace("🌐", "")
                .Replace("💾", "").Replace("🎭", "").Replace("👋", "")
                .Replace("•", "");
            return Task.Run(() => { try { _synth.Speak(clean); } catch { } });
        }

        public void SpeakGreeting(string userName)
        {
            if (!IsEnabled || _disposed) return;
            string greeting = string.IsNullOrEmpty(userName) || userName == "friend"
                ? "Welcome to CyberGuard, your cybersecurity awareness chatbot!"
                : string.Format("Welcome back, {0}! CyberGuard is ready to help you stay safe online.", userName);
            SpeakAsync(greeting);
        }

        public void Dispose()
        {
            if (!_disposed) { _synth.Dispose(); _disposed = true; }
        }
    }
}