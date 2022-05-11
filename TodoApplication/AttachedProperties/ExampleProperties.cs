using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TodoApplication.AttachedProperties
{
    internal class ExampleProperties 
    {

        public static readonly DependencyProperty SetBackgroundColorWhenEmptyProperty =
            DependencyProperty.RegisterAttached("SetBackgroundColorWhenEmpty", typeof(bool),
                typeof(ExampleProperties), new PropertyMetadata(false, OnSetBackgroundColorWhenEmptyChanged));

        public static readonly DependencyProperty TextEmptyBrushProperty =
            DependencyProperty.RegisterAttached("TextEmptyBrush", typeof(Brush),
                typeof(ExampleProperties), new PropertyMetadata(Brushes.Red));


        private static void OnSetBackgroundColorWhenEmptyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is TextBox textBox && GetSetBackgroundColorWhenEmpty(d))
            {
                textBox.TextChanged += TextBoxTextChanged;
            }
        }

        private static void TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            var emptyBrush = GetTextEmptyBrush(textBox);
            textBox.Background = GetBackgroundBrushByText(textBox.Text, emptyBrush);
        }

        internal static Brush GetBackgroundBrushByText(string textBoxText, Brush emptyBrush)
        {
            if (String.IsNullOrEmpty(textBoxText))
            {
                return emptyBrush;
            }
            else
            {
                return Brushes.White;
            }
        }

        public static bool GetSetBackgroundColorWhenEmpty(DependencyObject obj)
        {
            return (bool)obj.GetValue(SetBackgroundColorWhenEmptyProperty);
        }

        public static void SetSetBackgroundColorWhenEmpty(DependencyObject obj, bool value)
        {
            obj.SetValue(SetBackgroundColorWhenEmptyProperty, value);
        }

        public static Brush GetTextEmptyBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(TextEmptyBrushProperty);
        }

        public static void SetTextEmptyBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(TextEmptyBrushProperty, value);
        }
    }
}
