﻿using System.Globalization;

namespace BasicTaskManagement.UI.MAUI.Converters;

public class BoolToYesNoConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is null ? false : (bool)value ? "Yes" : "No";

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}
