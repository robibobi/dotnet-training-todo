using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TodoApplication.Converters
{
    internal class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // -> DateTime -> string
            if(value is DateTime timeStamp)
            {
                var formatString = parameter as string;
                return timeStamp.ToString(formatString);
            } else
            {
                return "unknown";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // string -> DateTime
            throw new NotImplementedException();
        }
    }
}
