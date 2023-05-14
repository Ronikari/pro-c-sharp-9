using System;
using System.Windows.Data;

namespace WpfControlsAndAPIs
{
    public class MyDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // Преобразовать значение double в int
            double v = (double)value;
            return (int)v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // Поскольку заботиться здесь о "двунаправленной" привязке не нужно, просто возвратить значение value
            return value;
        }
    }
}
