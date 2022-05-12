using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TodoApplication.Behaviors
{
    internal class ScrollViewerHelper
    {
        public static readonly DependencyProperty AutoScrollProperty =
            DependencyProperty.RegisterAttached("AutoScroll", typeof(bool),
                typeof(ScrollViewerHelper), new PropertyMetadata(false, OnAutoScrollChanged));

        private static void OnAutoScrollChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollChanged += OnScrollChanged;
            } else if(d is ListBox listbox)
            {
           
                listbox.Loaded += (s, loadedArgs) =>
                {
                    var childCount = VisualTreeHelper.GetChildrenCount(listbox);
                    var border = VisualTreeHelper.GetChild(listbox, 0);
                    var sv = VisualTreeHelper.GetChild(border, 0) as ScrollViewer;
                    sv.ScrollChanged += OnScrollChanged;
                };
            }
        }

        private static void OnScrollChanged(object sender, ScrollChangedEventArgs args)
        {
            var sv = sender as ScrollViewer;
            if (args.ExtentHeightChange > 0d)
            {
                sv.ScrollToBottom();
            }
        }

        public static bool GetAutoScroll(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoScrollProperty);
        }

        public static void SetAutoScroll(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoScrollProperty, value);
        }

    }
}
