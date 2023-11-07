using HelperExtensions;
using PropertyCreator2.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace PropertyCreator2.Converters;

public class SimplePropertyDataToPrivateVarDeclarationConverter : IValueConverter
{
    #region Convert
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is SimplePropertyData simplePropertyData)
        {
            string varName = simplePropertyData.Name.IsNullOrWhiteSpace() ? "VarName" : simplePropertyData.Name;
            return $"private {simplePropertyData.Type} _{Char.ToLower(varName[0])}{(varName.Length > 1 ? varName[1..] : "")}";
        }

        return String.Empty;
    }
    #endregion

    #region ConvertBack
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
    #endregion
}