using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Final440.Styles
{
    public static class ThemeApp
    {
        public static readonly Color Background = Color.FromArgb("#F3FAF5");     
        public static readonly Color CardBackground = Colors.White;
        public static readonly Color PrimaryGreen = Color.FromArgb("#2E7D32");   
        public static readonly Color AccentBlue = Color.FromArgb("#1E88E5");   
        public static readonly Color TextPrimary = Color.FromArgb("#1B4332"); 
        public static readonly Color TextSecondary = Color.FromArgb("#4F6F52");
        public static readonly Color BorderSoft = Color.FromArgb("#D0E2D7");

        public static void StylePage(ContentPage page)
        {
            page.BackgroundColor = Background;
        }

        public static Label CreateTitleLabel(string text)
        {
            return new Label
            {
                Text = text,
                FontSize = 26,
                FontAttributes = FontAttributes.Bold,
                TextColor = PrimaryGreen,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 0, 0, 8)
            };
        }

        public static Label CreateSectionHeader(string text)
        {
            return new Label
            {
                Text = text,
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                TextColor = TextPrimary,
                Margin = new Thickness(0, 12, 0, 4)
            };
        }

        public static Label CreateBodyLabel(string text = "")
        {
            return new Label
            {
                Text = text,
                FontSize = 14,
                TextColor = TextSecondary
            };
        }

        public static Label CreateBoldPrefixLabel(string prefix, string text)
        {
            return new Label
            {
                FormattedText = new FormattedString
                {
                    Spans =
            {
                new Span { Text = prefix, FontAttributes = FontAttributes.Bold },
                new Span { Text = text }
            }
                },
                FontSize = 14,
                TextColor = TextSecondary
            };
        }

        public static Button CreatePrimaryButton(string text)
        {
            return new Button
            {
                Text = text,
                BackgroundColor = PrimaryGreen,
                TextColor = Colors.White,
                CornerRadius = 24,
                FontSize = 16,
                Padding = new Thickness(18, 10),
                Margin = new Thickness(0, 4)
            };
        }

        public static Button CreateSecondaryButton(string text)
        {
            return new Button
            {
                Text = text,
                BackgroundColor = Colors.White,
                TextColor = PrimaryGreen,
                CornerRadius = 24,
                FontSize = 16,
                Padding = new Thickness(18, 10),
                Margin = new Thickness(0, 4),
                BorderColor = PrimaryGreen,
                BorderWidth = 1
            };
        }

        public static View WrapInCard(View content)
        {
            return new Frame
            {
                BackgroundColor = CardBackground,
                CornerRadius = 16,
                HasShadow = true,
                Padding = 16,
                Margin = new Thickness(0, 8),
                BorderColor = BorderSoft,
                Content = content
            };
        }
    }
}
