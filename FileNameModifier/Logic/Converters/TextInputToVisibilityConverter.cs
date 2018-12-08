using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FileNameModifier.Logic.Converters
{
    public class TextInputToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int length && values[1] is bool focused)
            {
                var hasText = length > 0;
                var hasFocus = focused;
                if (hasText || hasFocus)
                    return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
