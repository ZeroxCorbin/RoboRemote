using System;
using Xamarin.Forms;

namespace RoboRemote.Converters
{
    public class CheckByteValueConverter : IValueConverter
    {
        private object prevValue = "";
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            prevValue = value;
            return (string)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (string.IsNullOrEmpty((string)value))
                return value;
            if (byte.TryParse((string)value, out byte result))
            {
                return value;
            }
            else
                return prevValue;
        }
    }

    public class CheckInt16ValueConverter : IValueConverter
    {
        private object prevValue = "";
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            prevValue = value;
            return (string)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (string.IsNullOrEmpty((string)value))
                return value;
            if (Int16.TryParse((string)value, out Int16 result))
            {
                return value;
            }
            else
                return prevValue;
        }
    }

    public class CheckInt32ValueConverter : IValueConverter
    {
        private object prevValue = "";
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            prevValue = value;
            return (string)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (string.IsNullOrEmpty((string)value))
                return value;
            if(Int32.TryParse((string)value, out Int32 result))
            {
                return value;
            }
            else
                return prevValue;
        }
    }

    public class CheckFloatValueConverter : IValueConverter
    {
        private object prevValue = "";
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            prevValue = value;
            return (string)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (string.IsNullOrEmpty((string)value))
                return value;
            if (float.TryParse((string)value, out float result))
            {
                return value;
            }
            else
                return prevValue;
        }
    }

    public class CheckStringValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (string)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
