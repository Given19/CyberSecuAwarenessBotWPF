using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using CyberSecuAwarenessBotWPF.Services;
using CyberSecuAwarenessBotWPF.UI;

namespace CyberSecuAwarenessBotWPF
{
    public partial class MainWindow : Window, IDisposable
    {
        private readonly ChatbotService _engine = new ChatbotService();
        private readonly AudioPlayer _voice = new AudioPlayer();
        private bool _voiceEnabled = true;
        private bool _disposed;

        private const string AsciiArt =
            "  ██████╗██╗   ██╗██████╗ ███████╗██████╗ \n" +
            " ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗\n" +
            " ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝\n" +
            " ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗\n" +
            " ╚██████╗   ██║   ██████╔╝███████╗██║  ██║\n" +
            "  ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝\n" +
            "      🛡 GUARD YOUR DIGITAL WORLD 🛡";

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AsciiArtBlock.Text = AsciiArt;

            foreach (string topic in _engine.AvailableTopics)
            {
                var btn = new Button
                {
                    Content = topic,
                    Style = (Style)FindResource("CyberButtonSecondary"),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Margin = new Thickness(0, 0, 0, 4)
                };
                string t = topic;
                btn.Click += (s, args) => SendMessage(t);
                QuickTopicsPanel.Children.Add(btn);
            }

            _engine.OnSentimentDetected += OnSentimentDetected;
            _voice.SpeakGreeting(string.Empty);

            AppendBotMessage(
                "👋 Welcome to **CyberGuard** — your cybersecurity awareness assistant!\n\n" +
                "I can help you understand:\n" +
                "• 🔑 Password security\n• 🎣 Phishing & email scams\n" +
                "• 🔒 Online privacy\n• ⚠️ Scam detection\n" +
                "• 🦠 Malware & viruses\n• 📶 Wi-Fi security\n\n" +
                "_Tell me your name to get started, or click a topic on the left!_");

            InputBox.Focus();
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e) =>
            SendMessage(InputBox.Text);

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrWhiteSpace(InputBox.Text))
                SendMessage(InputBox.Text);
        }

        private void SendMessage(string text)
        {
            text = text.Trim();
            if (string.IsNullOrWhiteSpace(text)) return;

            AppendUserMessage(text);
            InputBox.Clear();
            ConsoleUI.ShowStatus(StatusBar, "CyberGuard is thinking…");

            Task.Run(() => _engine.ProcessInput(text))
                .ContinueWith(t =>
                {
                    string response = t.Result;
                    AppendBotMessage(response);
                    UpdateSidebar();
                    ConsoleUI.ShowStatus(StatusBar, "Ready.");
                    if (_voiceEnabled) _voice.SpeakAsync(response);
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AppendUserMessage(string text)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                string name = _engine.UserName == "friend" ? "You" : _engine.UserName;
                ChatPanel.Children.Add(new ChatBubble(text, isUser: true, senderLabel: name));
                ScrollToBottom();
            }));
        }

        private void AppendBotMessage(string text)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var bubble = new ChatBubble(text, isUser: false, senderLabel: "🛡 CyberGuard");
                bubble.Opacity = 0;
                ChatPanel.Children.Add(bubble);
                var anim = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
                bubble.BeginAnimation(OpacityProperty, anim);
                ScrollToBottom();
            }));
        }

        private void ScrollToBottom()
        {
            ChatScrollViewer.UpdateLayout();
            ChatScrollViewer.ScrollToBottom();
        }

        private void UpdateSidebar()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                LblUserName.Text = _engine.UserName == "friend" ? "Not set" : _engine.UserName;
                LblUserInterest.Text = string.IsNullOrEmpty(_engine.UserInterest) ? "No topic yet" : _engine.UserInterest;
            }));
        }

        private void OnSentimentDetected(string sentiment, string topic)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                ConsoleUI.UpdateMood(MoodEmoji, MoodLabel, sentiment);
            }));
        }

        private void BtnVoice_Click(object sender, RoutedEventArgs e)
        {
            _voiceEnabled = !_voiceEnabled;
            _voice.IsEnabled = _voiceEnabled;
            BtnVoice.Content = _voiceEnabled ? "🔊 Voice" : "🔇 Voice";
            ConsoleUI.ShowStatus(StatusBar, _voiceEnabled ? "Voice responses enabled." : "Voice responses disabled.");
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ConsoleUI.ClearChat(ChatPanel);
            AppendBotMessage("🧹 Chat cleared! How can I help you today?");
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e) =>
            SendMessage("help");

        private void InputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            InputPlaceholder.Visibility =
                string.IsNullOrEmpty(InputBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        public void Dispose()
        {
            if (!_disposed) { _voice.Dispose(); _disposed = true; }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Dispose();
        }
    }
}