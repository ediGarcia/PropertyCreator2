using PropertyCreator2.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace PropertyCreator2.Converters;

public class PropertyClassToStringConverter : IValueConverter
{
    #region Convert
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is DependencyPropertyData ? "Dependency Property" : "Property";
    #endregion

    #region ConvertBack
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
    #endregion
}
