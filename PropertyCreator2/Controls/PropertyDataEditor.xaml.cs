using PropertyCreator2.Enums;
using PropertyCreator2.Models;
using System.ComponentModel;
using System.Windows;
#pragma warning disable CS8602
#pragma warning disable CS8600

namespace PropertyCreator2.Controls;

/// <summary>
/// Interaction logic for PropertyDataEditor.xaml
/// </summary>
public partial class PropertyDataEditor
{
    #region Properties

    /// <summary>
    /// Gets or sets the current property data.
    /// </summary>
    [Category("Behaviour")]
    [Description("Gets or sets the current property data.")]
    public PropertyData PropertyData
    {
        get => (PropertyData)GetValue(PropertyDataProperty);
        set
        {
            if (PropertyData is DependencyPropertyData)
                PropertyData.PropertyChanged -= PropertyData_PropertyChanged;

            SetValue(PropertyDataProperty, value);

            if (PropertyData is DependencyPropertyData dependencyPropertyData)
            {
                PropertyData.PropertyChanged += PropertyData_PropertyChanged;
                dependencyPropertyData.AttributeDefaultValueType ??= dependencyPropertyData.Type;
            }
        }
    }

    public static readonly DependencyProperty PropertyDataProperty = DependencyProperty.Register(
        nameof(PropertyData),
        typeof(PropertyData),
        typeof(PropertyDataEditor),
        new FrameworkPropertyMetadata(
            null,
            FrameworkPropertyMetadataOptions.AffectsRender));

    #endregion

    public PropertyDataEditor()
    {
        InitializeComponent();

        CmbAccessModifier.ItemsSource = new[]
        {
            new { Text = "internal", Value = AccessModifier.Internal },
            new { Text = "private", Value = AccessModifier.Private },
            new { Text = "private protected", Value = AccessModifier.PrivateProtected },
            new { Text = "protected", Value = AccessModifier.Protected },
            new { Text = "protected internal", Value = AccessModifier.ProtectedInternal },
            new { Text = "public", Value = AccessModifier.Public }
        };

        CmbType.ItemsSource = new[]
        {
            "bool",
            "Brush",
            "byte",
            "char",
            "CornerRadius",
            "DateTime",
            "decimal",
            "Dictionary<,>",
            "double",
            "dynamic",
            "float",
            "int",
            "List<>",
            "long",
            "object",
            "sbyte",
            "short",
            "string",
            "Thickness",
            "TimeSpan",
            "uint",
            "ulong",
            "ushort"
        };

        object[] modifiers =
        {
            new { Text = "None", Value = PropertyMethodStatus.None },
            new { Text = "private", Value = PropertyMethodStatus.Private },
            new { Text = "public", Value = PropertyMethodStatus.Public }
        };

        CmbGetter.ItemsSource = modifiers;
        CmbSetter.ItemsSource = modifiers;
    }

    #region Events

    #region PropertyDataEditor_Loaded
    /// <summary>
    /// Focus on the first element of the control when finished loading.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PropertyDataEditor_Loaded(object sender, RoutedEventArgs e) =>
        CmbType.Focus();
    #endregion

    #region PropertyData_PropertyChanged
    /// <summary>
    /// Updates the attribute values according to the type and default value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PropertyData_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        DependencyPropertyData dependencyPropertyData = PropertyData as DependencyPropertyData;

        switch (e.PropertyName)
        {
            case nameof(dependencyPropertyData.Type):
                dependencyPropertyData.AttributeDefaultValueType =
                    dependencyPropertyData.Type == "Brush" ? "Brushes" : dependencyPropertyData.Type;
                break;

            case nameof(dependencyPropertyData.DefaultValue):
                dependencyPropertyData.AttributeDefaultValue = dependencyPropertyData.DefaultValue;
                break;
        }
    }
    #endregion

    #endregion
}