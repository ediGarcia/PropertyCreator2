using HelperExtensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace PropertyCreator2.Converters;

public class StringLengthToBooleanConverter : IValueConverter
{
    #region Convert
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is not null && !value.ToString().IsNullOrWhiteSpace();
    #endregion

    #region ConvertBack
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
    #endregion
}