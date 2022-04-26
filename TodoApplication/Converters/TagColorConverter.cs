using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TodoApplication.Models;

namespace TodoApplication.Converters
{
    internal class TagColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is TagColor tagColor)
            {
                switch (tagColor)
                {
                    case TagColor.Color1:
                        return Brushes.Red;
                    case TagColor.Color2:
                        return Brushes.Green;
                    case TagColor.Color3:
                        return Brushes.Blue;
                    case TagColor.Color4:
                        return Brushes.HotPink;
                }
            }
            return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
