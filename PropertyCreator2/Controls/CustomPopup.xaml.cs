using PropertyCreator2.Models;
using System;
using System.ComponentModel;
using System.Windows;

namespace PropertyCreator2.Controls;

/// <summary>
/// Interaction logic for CustomPopup.xaml
/// </summary>
public partial class CustomPopup
{
    #region Custom Events

    /// <summary>
    /// Notifies whenever the user presses the "Cancel" button.
    /// </summary>
    public event EventHandler<PopupButtonEventArgs> PopupCancelled;

    /// <summary>
    /// Notifies whenever the user presses the "OK" button.
    /// </summary>
    public event EventHandler<PopupButtonEventArgs> PopupConfirmed;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the content of the <see cref="CustomPopup"/>.
    /// </summary>
    [Category("Behaviour")]
    [Description("Gets or sets the content of the CustomPopup.")]
    public new object Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    /// <summary>
    /// Gets or sets the horizontal alignment characteristics applied to this element when it is composed within a parent element, such as a a panel or items control.
    /// </summary>
    [Category("Appearance")]
    [DefaultValue(typeof(HorizontalAlignment), "Center")]
    [Description("Gets or sets the horizontal alignment characteristics applied to this element when it is composed within a parent element, such as a a panel or items control.")]
    public new HorizontalAlignment HorizontalAlignment
    {
        get => (HorizontalAlignment)GetValue(HorizontalAlignmentProperty);
        set => SetValue(HorizontalAlignmentProperty, value);
    }

    /// <summary>
    /// Gets or sets whether the cancel button is visible.
    /// </summary>
    [Category("Appearance")]
    [DefaultValue(true)]
    [Description("Gets or sets whether the cancel button is visible.")]
    public bool IsCancelButtonVisible
    {
        get => (bool)GetValue(IsCancelButtonVisibleProperty);
        set => SetValue(IsCancelButtonVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets whether the popup is visible.
    /// </summary>
    [Category("Appearance")]
    [DefaultValue(false)]
    [Description("Gets or sets whether the popup is visible.")]
    public bool IsOpen
    {
        get => (bool)GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    /// <summary>
    /// Gets or sets the popup title.
    /// </summary>
    [Category("Appearance")]
    [DefaultValue("Title")]
    [Description("Gets or sets the popup title.")]
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the vertical alignment characteristics applied to this element when it is composed within a parent element, such as a a panel or items control.
    /// </summary>
    [Category("Appearance")]
    [DefaultValue(typeof(VerticalAlignment), "Center")]
    [Description("Gets or sets the vertical alignment characteristics applied to this element when it is composed within a parent element, such as a a panel or items control.")]
    public new VerticalAlignment VerticalAlignment
    {
        get => (VerticalAlignment)GetValue(VerticalAlignmentProperty);
        set => SetValue(VerticalAlignmentProperty, value);
    }

    /// <summary>
    /// Gets or sets the popup title.
    /// </summary>
    [Category("Appearance")]
    [DefaultValue(Double.NaN)]
    [Description("Gets or sets the popup title.")]
    public new double Width
    {
        get => (double)GetValue(WidthProperty);
        set => SetValue(WidthProperty, value);
    }

    public new static readonly DependencyProperty ContentProperty =
        DependencyProperty.Register(
            nameof(Content),
            typeof(object),
            typeof(CustomPopup),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public new static readonly DependencyProperty HorizontalAlignmentProperty =
        DependencyProperty.Register(
            nameof(HorizontalAlignment),
            typeof(HorizontalAlignment),
            typeof(CustomPopup),
            new FrameworkPropertyMetadata(
                HorizontalAlignment.Center,
                FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IsCancelButtonVisibleProperty =
        DependencyProperty.Register(
            nameof(IsCancelButtonVisible),
            typeof(bool),
            typeof(CustomPopup),
            new FrameworkPropertyMetadata(
                true,
                FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IsOpenProperty =
        DependencyProperty.Register(
            nameof(IsOpen),
            typeof(bool),
            typeof(CustomPopup),
            new FrameworkPropertyMetadata(
                false,
                FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(CustomPopup),
            new FrameworkPropertyMetadata(
                "Title",
                FrameworkPropertyMetadataOptions.AffectsRender));

    public new static readonly DependencyProperty VerticalAlignmentProperty =
        DependencyProperty.Register(
            nameof(VerticalAlignment),
            typeof(VerticalAlignment),
            typeof(CustomPopup),
            new FrameworkPropertyMetadata(
                VerticalAlignment.Center,
                FrameworkPropertyMetadataOptions.AffectsRender));

    public new static readonly DependencyProperty WidthProperty =
        DependencyProperty.Register(
            nameof(Width),
            typeof(double),
            typeof(CustomPopup),
            new FrameworkPropertyMetadata(
                Double.NaN,
                FrameworkPropertyMetadataOptions.AffectsRender));

    #endregion

    public CustomPopup() =>
        InitializeComponent();

    #region Events

    #region BtnCancel_OnClick
    /// <summary>
    /// Notifies the listeners that the "Cancel" button was clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
    {
        PopupButtonEventArgs eventArgs = new();
        PopupCancelled?.Invoke(this, eventArgs);

        if (!eventArgs.Cancel)
            IsOpen = false;
    }
    #endregion

    #region BtnOk_OnClick
    /// <summary>
    /// Notifies the listeners that the "OK" button was clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnOk_OnClick(object sender, RoutedEventArgs e)
    {
        PopupButtonEventArgs eventArgs = new();
        PopupConfirmed?.Invoke(this, eventArgs);

        if (!eventArgs.Cancel)
            IsOpen = false;
    }
    #endregion

    #endregion
}