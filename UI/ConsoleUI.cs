using System.Windows.Controls;
using System.Windows.Media;

namespace CyberSecuAwarenessBotWPF.UI
{
    public static class ConsoleUI
    {
        public static void ShowStatus(TextBlock statusBar, string message)
        {
            statusBar.Text = message;
        }

        public static void ClearChat(StackPanel chatPanel)
        {
            chatPanel.Children.Clear();
        }

        public static void UpdateMood(TextBlock moodEmoji, TextBlock moodLabel, string sentiment)
        {
            switch (sentiment)
            {
                case "Worried":
                    moodEmoji.Text = "😟";
                    moodLabel.Text = "Worried";
                    moodLabel.Foreground = new SolidColorBrush(Color.FromRgb(0xF8, 0x51, 0x49));
                    break;
                case "Curious":
                    moodEmoji.Text = "🤔";
                    moodLabel.Text = "Curious";
                    moodLabel.Foreground = new SolidColorBrush(Color.FromRgb(0x58, 0xA6, 0xFF));
                    break;
                case "Frustrated":
                    moodEmoji.Text = "😤";
                    moodLabel.Text = "Frustrated";
                    moodLabel.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xA6, 0x57));
                    break;
                case "Positive":
                    moodEmoji.Text = "😊";
                    moodLabel.Text = "Positive";
                    moodLabel.Foreground = new SolidColorBrush(Color.FromRgb(0x39, 0xD3, 0x53));
                    break;
                default:
                    moodEmoji.Text = "😐";
                    moodLabel.Text = "Neutral";
                    moodLabel.Foreground = new SolidColorBrush(Color.FromRgb(0x8B, 0x94, 0x9E));
                    break;
            }
        }
    }
}