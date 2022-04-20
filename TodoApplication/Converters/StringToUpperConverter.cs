using System;
using System.Globalization;
using System.Windows.Data;

namespace TodoApplication.Converters
{
    internal class StringToUpperConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return string.Empty;
            } else
            {
                return value.ToString().ToUpper(CultureInfo.InvariantCulture);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
