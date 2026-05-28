using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace CyberSecuAwarenessBotWPF
{
    public class ChatBubble : Border
    {
        private static readonly SolidColorBrush BotBg = new SolidColorBrush(Color.FromRgb(0x21, 0x26, 0x2D));
        private static readonly SolidColorBrush UserBg = new SolidColorBrush(Color.FromRgb(0x0D, 0x4A, 0x1E));
        private static readonly SolidColorBrush BotFg = new SolidColorBrush(Color.FromRgb(0xE6, 0xED, 0xF3));
        private static readonly SolidColorBrush UserFg = new SolidColorBrush(Color.FromRgb(0xE6, 0xED, 0xF3));
        private static readonly SolidColorBrush TimeFg = new SolidColorBrush(Color.FromRgb(0x8B, 0x94, 0x9E));
        private static readonly SolidColorBrush BoldFg = new SolidColorBrush(Color.FromRgb(0x58, 0xA6, 0xFF));

        public ChatBubble(string message, bool isUser, string senderLabel = "")
        {
            CornerRadius = isUser ? new CornerRadius(12, 12, 2, 12) : new CornerRadius(12, 12, 12, 2);
            Background = isUser ? UserBg : BotBg;
            Padding = new Thickness(14, 10, 14, 10);
            MaxWidth = 620;
            Margin = isUser ? new Thickness(80, 0, 0, 0) : new Thickness(0, 0, 80, 0);
            HorizontalAlignment = isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left;

            Effect = new System.Windows.Media.Effects.DropShadowEffect
            { Color = Colors.Black, BlurRadius = 8, ShadowDepth = 2, Opacity = 0.25 };

            var stack = new StackPanel();

            if (!string.IsNullOrEmpty(senderLabel))
                stack.Children.Add(new TextBlock
                {
                    Text = senderLabel,
                    Foreground = isUser ? new SolidColorBrush(Color.FromRgb(0x39, 0xD3, 0x53)) : BoldFg,
                    FontSize = 10,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 0, 4)
                });

            stack.Children.Add(BuildRichText(message, isUser ? UserFg : BotFg));

            stack.Children.Add(new TextBlock
            {
                Text = DateTime.Now.ToString("HH:mm"),
                Foreground = TimeFg,
                FontSize = 10,
                HorizontalAlignment = isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                Margin = new Thickness(0, 4, 0, 0)
            });

            Child = stack;
        }

        private static TextBlock BuildRichText(string text, SolidColorBrush defaultFg)
        {
            var tb = new TextBlock
            {
                Foreground = defaultFg,
                FontSize = 13,
                TextWrapping = TextWrapping.Wrap,
                LineHeight = 20
            };

            string[] parts = text.Split(new string[] { "**" }, StringSplitOptions.None);
            for (int i = 0; i < parts.Length; i++)
            {
                if (i % 2 == 1)
                {
                    tb.Inlines.Add(new Run(parts[i]) { FontWeight = FontWeights.Bold, Foreground = BoldFg });
                }
                else
                {
                    string[] italicParts = parts[i].Split('_');
                    for (int j = 0; j < italicParts.Length; j++)
                    {
                        if (j % 2 == 1)
                            tb.Inlines.Add(new Run(italicParts[j]) { FontStyle = FontStyles.Italic, Foreground = TimeFg });
                        else
                            tb.Inlines.Add(new Run(italicParts[j]));
                    }
                }
            }
            return tb;
        }
    }
}