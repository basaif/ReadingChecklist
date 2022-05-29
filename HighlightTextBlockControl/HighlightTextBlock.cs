using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace HighlightTextBlockControl
{
    [TemplatePart(Name = TEXT_DISPLAY_PART_NAME, Type = typeof(TextBlock))]
    public class HighlightTextBlock : Control
    {
        private const string TEXT_DISPLAY_PART_NAME = "PART_TextDisplay";

        private TextBlock _displayTextBlock = new();

        public static readonly DependencyProperty HighlightTextProperty =
            DependencyProperty.Register("HighlightText", typeof(string), typeof(HighlightTextBlock),
                new PropertyMetadata(string.Empty, OnHighlightTextPropertyChanged));

        public string HighlightText
        {
            get { return (string)GetValue(HighlightTextProperty); }
            set { SetValue(HighlightTextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            TextBlock.TextProperty.AddOwner(typeof(HighlightTextBlock),
                new PropertyMetadata(string.Empty, OnHighlightTextPropertyChanged));



        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextWrapping.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(HighlightTextBlock), new PropertyMetadata(TextWrapping.NoWrap));



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty HighlightRunStyleProperty =
            DependencyProperty.Register("HighlightRunStyle", typeof(Style), typeof(HighlightTextBlock),
                new PropertyMetadata(CreateDefaultHighlightRunStyle()));

        private static object CreateDefaultHighlightRunStyle()
        {
            Style style = new(typeof(Run));
            style.Setters.Add(new Setter(BackgroundProperty, Brushes.Yellow));

            return style;
        }

        public Style HighlightRunStyle
        {
            get { return (Style)GetValue(HighlightRunStyleProperty); }
            set { SetValue(HighlightRunStyleProperty, value); }
        }

        static HighlightTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HighlightTextBlock), new FrameworkPropertyMetadata(typeof(HighlightTextBlock)));
        }

        public override void OnApplyTemplate()
        {
            if (Template.FindName(TEXT_DISPLAY_PART_NAME, this) is TextBlock textBlock)
            {
                _displayTextBlock = textBlock;
                UpdateHighlightDisplay();
            }

            base.OnApplyTemplate();
        }

        private static void OnHighlightTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HighlightTextBlock highlightTextBlock)
            {
                highlightTextBlock.UpdateHighlightDisplay();
            }
        }

        private void UpdateHighlightDisplay()
        {
            if (_displayTextBlock != null)
            {
                _displayTextBlock.Inlines.Clear();

                int highlightTextLength = HighlightText.Length;
                if (highlightTextLength == 0)
                {
                    _displayTextBlock.Text = Text;
                }
                else
                {

                    for (int i = 0; i < Text.Length; i++)
                    {
                        if (i + highlightTextLength > Text.Length)
                        {
                            _displayTextBlock.Inlines.Add(new Run(Text[i..]));
                            break;
                        }

                        int nextHighlightTextIndex = Text.IndexOf(HighlightText, i, StringComparison.OrdinalIgnoreCase);
                        if (nextHighlightTextIndex == -1)
                        {
                            _displayTextBlock.Inlines.Add(new Run(Text[i..]));
                            break;
                        }

                        _displayTextBlock.Inlines.Add(new Run(Text[i..nextHighlightTextIndex]));
                        _displayTextBlock.Inlines.Add(CreateHighlightedRun(HighlightText.ToLower()));

                        i = nextHighlightTextIndex + highlightTextLength - 1;
                    }
                }
            }
        }


        private Run CreateHighlightedRun(string text)
        {
            return new Run(GetHighlightedTextInText(text))
            {
                Style = HighlightRunStyle
            };
        }

        private string GetHighlightedTextInText(string text)
        {
            return Text.Substring(Text.IndexOf(text, StringComparison.OrdinalIgnoreCase), text.Length);
        }
    }
}
