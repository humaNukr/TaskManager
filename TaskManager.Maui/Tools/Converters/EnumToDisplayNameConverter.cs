using KMA.TaskManager.Common;
using System.Globalization;

namespace KMA.TaskManager.Maui.Tools.Converters
{
    public class EnumToDisplayNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return string.Empty;
            if (value is not Enum castedEnum) return value.ToString() ?? string.Empty;

            return castedEnum.GetDisplayName();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}