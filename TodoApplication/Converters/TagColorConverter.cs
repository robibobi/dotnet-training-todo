using System;
using System.Globalization;
using System.Windows;
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
                        return GetTagBrush("TagColor1");
                    case TagColor.Color2:
                        return GetTagBrush("TagColor2");
                    case TagColor.Color3:
                        return GetTagBrush("TagColor3");
                    case TagColor.Color4:
                        return GetTagBrush("TagColor4");
                }
            }
            return GetTagBrush("TagColorDefault");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


        private SolidColorBrush GetTagBrush(string resourceKey)
        {
            var brushResource = Application.Current.Resources[resourceKey];
            return brushResource as SolidColorBrush;
        }

    }
}
