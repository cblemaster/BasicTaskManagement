using System.Globalization;
using System.Windows.Data;

namespace BasicTaskManagement.WPF.Converters;

internal class IsAddTaskGroupButtonEnabledConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) =>
        values.ToList().All(v => v.GetType() == typeof(bool))
            && !(bool)values[0] && !(bool)values[1] && !(bool)values[2] && !(bool)values[3];

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
