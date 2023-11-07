using HelperExtensions;
using HelperMethods;
using PropertyCreator2.Controls;
using PropertyCreator2.Models;
using PropertyCreator2.Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
#pragma warning disable CS8625
#pragma warning disable CS8602
#pragma warning disable CS8601

namespace PropertyCreator2;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    #region Properties

    /// <summary>
    /// Gets the current application settings.
    /// </summary>
    public Settings Settings { get; }

    #endregion

    private const string DataFile = "data.dat";

    private readonly PropertyDataEditor _propertyDataEditor;
    private PropertyData _selectedPropertyData;

    public MainWindow()
    {
        InitializeComponent();

        try
        {
            Settings = SystemMethods.RetrieveDataFromFile<Settings>(DataFile);
        }
        catch
        {
            Settings = new();
        }

        _propertyDataEditor = PopPopup.Content as PropertyDataEditor;
    }

    #region Events

    #region BtnAddDependencyProperty_OnClick
    /// <summary>
    /// Opens the Dependency Property popup.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnAddDependencyProperty_OnClick(object sender, RoutedEventArgs e)
    {
        _propertyDataEditor.PropertyData = new DependencyPropertyData { OwnerType = Settings.LastUsedOwnerType };
        PopPopup.Title = "Add Dependency Property";
        PopPopup.IsOpen = true;
    }
    #endregion

    #region BtnAddProperty_OnClick
    /// <summary>
    /// Opens the Simple Property popup.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnAddProperty_OnClick(object sender, RoutedEventArgs e)
    {
        _propertyDataEditor.PropertyData = new SimplePropertyData();
        PopPopup.Title = "Add Property";
        PopPopup.IsOpen = true;
    }
    #endregion

    #region BtnGenerate_OnClick
    /// <summary>
    /// Generates the properties declaration.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    private void BtnGenerate_OnClick(object sender, RoutedEventArgs e)
    {
        (PopResult.Content as TextBox).Text = PropertyGeneratorService.GeneratePropertiesDeclaration(Settings.Properties);
        PopResult.IsOpen = true;
    }
    #endregion

    #region PopDelete_OnPopupCancelled
    /// <summary>
    /// Closes the delete popup message.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PopDelete_OnPopupCancelled(object? sender, PopupButtonEventArgs e) =>
        _selectedPropertyData = null;
    #endregion

    #region PopDelete_OnPopupConfirmed
    /// <summary>
    /// Deletes the selected property.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PopDelete_OnPopupConfirmed(object? sender, PopupButtonEventArgs e)
    {
        Settings.Properties.Remove(_selectedPropertyData);
        _selectedPropertyData = null;

        SystemMethods.SaveDataToFile(DataFile, Settings);
    }
    #endregion

    #region PopDeleteAll_OnPopupConfirmed
    /// <summary>
    /// Removes every property.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PopDeleteAll_OnPopupConfirmed(object? sender, PopupButtonEventArgs e)
    {
        Settings.Properties.Clear();
        SystemMethods.SaveDataToFile(DataFile, Settings);
    }
    #endregion

    #region PopPopup_OnPopupCancelled
    /// <summary>
    /// Clears the selected property data.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PopPopup_OnPopupCancelled(object? sender, PopupButtonEventArgs e) =>
        _selectedPropertyData = null;
    #endregion

    #region PopPopup_PopupConfirmed
    /// <summary>
    /// Confirms the property data.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PopPopup_OnPopupConfirmed(object? sender, PopupButtonEventArgs e)
    {
        if (_propertyDataEditor.PropertyData.Name.IsNullOrWhiteSpace()
            || _propertyDataEditor.PropertyData.Type.IsNullOrWhiteSpace())
        {
            PopWarningPopup.IsOpen = true;
            e.Cancel = true;
        }
        else
        {
            if (_selectedPropertyData is null)
                Settings.Properties.Add(_propertyDataEditor.PropertyData);
            else
            {
                _selectedPropertyData.Set(_propertyDataEditor.PropertyData);
                _selectedPropertyData = null;
            }

            if (_propertyDataEditor.PropertyData is DependencyPropertyData dependencyPropertyData)
                Settings.LastUsedOwnerType = dependencyPropertyData.OwnerType;

            SortProperties();
            SystemMethods.SaveDataToFile(DataFile, Settings);
        }
    }
    #endregion

    #region PropertyViewer_OnDeleteButtonClicked
    /// <summary>
    /// Shows the delete popup message.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PropertyViewer_OnRemoveButtonClicked(object? sender, PropertyViewerEventArgs e)
    {
        _selectedPropertyData = e.CurrentPropertyData;
        ((PopDelete.Content as Grid).Children[1] as TextBlock).Text =
            $"The \"{_selectedPropertyData.Name}\" {(_selectedPropertyData is DependencyPropertyData ? "dependency property" : "property")} will be permanently removed.";

        PopDelete.IsOpen = true;
    }
    #endregion

    #region PropertyViewer_OnEditButtonClicked
    /// <summary>
    /// Edits the currently selected property.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PropertyViewer_OnEditButtonClicked(object? sender, PropertyViewerEventArgs e)
    {
        _selectedPropertyData = e.CurrentPropertyData;
        _propertyDataEditor.PropertyData = _selectedPropertyData.Clone();

        PopPopup.Title = $"Edit {(_selectedPropertyData is DependencyPropertyData ? "Dependency Property" : "Property")}";
        PopPopup.IsOpen = true;
    }
    #endregion

    #region BtnRemoveAll_OnClick
    /// <summary>
    /// Shows the delete all popup.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnRemoveAll_OnClick(object sender, RoutedEventArgs e) =>
        PopDeleteAll.IsOpen = true;
    #endregion

    #endregion

    #region Private Methods

    #region SortProperties
    /// <summary>
    /// Sorts the properties.
    /// </summary>
    private void SortProperties()
    {
        List<PropertyData> orderedProperties = Settings.Properties.OrderBy(_ => _.Name).ThenBy(_ => _.Type).ToList();
        Settings.Properties.Clear();
        Settings.Properties.AddRange(orderedProperties);
    }
    #endregion

    #endregion
}