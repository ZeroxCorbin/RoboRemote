using System;
using TM_Comms;
using Xamarin.Forms;

namespace RoboRemote.Converters
{
    public class TMflowVersionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TMflowVersions ver = (TMflowVersions)Enum.Parse(typeof(TMflowVersions), (string)value);

            return ver == TMflowVersions.V1_76_xxxx || ver == TMflowVersions.V1_80_xxxx;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
