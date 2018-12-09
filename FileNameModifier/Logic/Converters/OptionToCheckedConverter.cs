using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using FileNameModifier.Logic.Constants;
using FileNameModifier.Logic.Enumerations;

namespace FileNameModifier.Logic.Converters
{
    class OptionToCheckedConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is string content) || !(values[1] is DeletionOption option))
                return false;
            if (content.Equals(Strings.CaptionOptionRemoveFirst) && option == DeletionOption.RemoveFirst)
                return true;
            return content.Equals(Strings.CaptionOptionRemoveAllOccurrences) && option == DeletionOption.RemoveAll;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
