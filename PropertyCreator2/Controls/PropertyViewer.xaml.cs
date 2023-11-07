using System;
using PropertyCreator2.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace PropertyCreator2.Controls;

/// <summary>
/// Interaction logic for SimplePropertyViewer.xaml
/// </summary>
public partial class PropertyViewer
{
    #region Custom Events

    /// <summary>
    /// Notifies whenever the "Edit" button is clicked.
    /// </summary>
    public event EventHandler<PropertyViewerEventArgs> EditButtonClicked;

    /// <summary>
    /// Notifies whenever the "Delete" button is clicked.
    /// </summary>
    public event EventHandler<PropertyViewerEventArgs> DeleteButtonClicked;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the brush that describes the background of the control when the mouse is hovering over it.
    /// </summary>
    [Category("Appearance")]
    [DefaultValue(typeof(Brushes), "LightGray")]
    [Description("Gets or sets the brush that describes the background of the control when the mouse is hovering over it.")]
    public Brush MouseOverBrush
    {
        get => (Brush)GetValue(MouseOverBrushProperty);
        set => SetValue(MouseOverBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="Models.PropertyData"/> of the control.
    /// </summary>
    [Category("Appearance")]
    [Description("Gets or sets the property data of the control.")]
    public PropertyData PropertyData
    {
        get => (PropertyData)GetValue(PropertyDataProperty);
        set => SetValue(PropertyDataProperty, value);
    }

    public static readonly DependencyProperty MouseOverBrushProperty = DependencyProperty.Register(
        nameof(MouseOverBrush),
        typeof(Brush),
        typeof(PropertyViewer),
        new FrameworkPropertyMetadata(
            Brushes.LightGray,
            FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty PropertyDataProperty = DependencyProperty.Register(
        nameof(PropertyData),
        typeof(PropertyData),
        typeof(PropertyViewer),
        new FrameworkPropertyMetadata(
            null,
            FrameworkPropertyMetadataOptions.AffectsRender));

    #endregion

    public PropertyViewer() =>
        InitializeComponent();

    #region Events

    #region BtnDelete_Click
    /// <summary>
    /// Notifies that the "Delete" button has been clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnDelete_Click(object sender, RoutedEventArgs e) =>
        DeleteButtonClicked?.Invoke(this, new(PropertyData));
    #endregion

    #region BtnEdit_Click
    /// <summary>
    /// Notifies that the "Edit" button has been clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnEdit_Click(object sender, RoutedEventArgs e) =>
        EditButtonClicked?.Invoke(this, new(PropertyData));
    #endregion

    #endregion
}