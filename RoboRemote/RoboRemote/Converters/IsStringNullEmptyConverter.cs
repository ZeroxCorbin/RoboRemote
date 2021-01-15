using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RoboRemote.Converters
{
    public class IsStringNullEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !string.IsNullOrEmpty($"{value}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
