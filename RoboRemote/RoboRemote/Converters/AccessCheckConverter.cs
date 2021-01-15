using RoboRemote.Interfaces;
using System;
using Xamarin.Forms;

namespace RoboRemote.Converters
{
    public class IsWriteableBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((IItem)value).Type == "bool" && (((IItem)value).Access == "W" || ((IItem)value).Access == "RW");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }

    public class IsNotWriteableBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((IItem)value).Type != "bool" && (((IItem)value).Access == "W" || ((IItem)value).Access == "RW");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
