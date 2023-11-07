using PropertyCreator2.Enums;
using System;
using System.ComponentModel;

namespace PropertyCreator2.Models;

[Serializable]
public abstract class PropertyData : INotifyPropertyChanged
{
    #region Custom Events

    /// <summary>
    /// Notifies whenever an specified property has its value changed.
    /// </summary>
    [field:NonSerialized]
    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the property's access modifier.
    /// </summary>
    public AccessModifier AccessModifier
    {
        get => _accessModifier;
        set
        {
            _accessModifier = value;
            NotifyPropertyChange(nameof(AccessModifier));
        }
    }

    /// <summary>
    /// Gets or sets the property's category.
    /// </summary>
    public string Category
    {
        get => _category;
        set
        {
            _category = value;
            NotifyPropertyChange(nameof(Category));
        }
    }

    /// <summary>
    /// Gets or sets the property's optional default value.
    /// </summary>
    public string DefaultValue
    {
        get => _defaultValue;
        set
        {
            _defaultValue = value;
            NotifyPropertyChange(nameof(DefaultValue));
        }
    }

    /// <summary>
    /// Gets or sets the property's description.
    /// </summary>
    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            NotifyPropertyChange(nameof(Description));
        }
    }

    /// <summary>
    /// Gets and sets the status of the property's get method.
    /// </summary>
    public PropertyMethodStatus GetStatus
    {
        get => _getStatus;
        set
        {
            _getStatus = value;
            NotifyPropertyChange(nameof(GetStatus));
        }
    }

    /// <summary>
    /// Gets and sets the property's name.
    /// </summary>
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            NotifyPropertyChange(nameof(Name));
        }
    }

    /// <summary>
    /// Gets and sets the status of the property's set method.
    /// </summary>
    public PropertyMethodStatus SetStatus
    {
        get => _setStatus;
        set
        {
            _setStatus = value;
            NotifyPropertyChange(nameof(SetStatus));
        }
    }

    /// <summary>
    /// Gets or sets the property type.
    /// </summary>
    public string Type
    {
        get => _type;
        set
        {
            _type = value;
            NotifyPropertyChange(nameof(Type));
        }
    }

    /// <summary>
    /// Gets or sets whether the description attribute should be added to the property declaration.
    /// </summary>
    public bool UseDescriptionAttribute
    {
        get => _useDescriptionAttribute;
        set
        {
            _useDescriptionAttribute = value;
            NotifyPropertyChange(nameof(UseDescriptionAttribute));
        }
    }

    #endregion

    private AccessModifier _accessModifier;
    private string _category = "None";
    private string _defaultValue;
    private string _description;
    private PropertyMethodStatus _getStatus;
    private string _name;
    private PropertyMethodStatus _setStatus;
    private string _type = "int";
    private bool _useDescriptionAttribute;

    #region Public Methods

    /// <summary>
    /// Creates a clone of the current instance.
    /// </summary>
    public abstract PropertyData Clone();

    /// <summary>
    /// Sets the current <see cref="PropertyData"/> with the specified values.
    /// </summary>
    public abstract void Set(PropertyData propertyData);

    #endregion

    #region Protected Methods

    #region NotifyPropertyChange
    /// <summary>
    /// Notifies whether the specified property value is changed.
    /// </summary>
    /// <param name="propertyName"></param>
    protected void NotifyPropertyChange(string propertyName) =>
        PropertyChanged?.Invoke(this, new(propertyName));
    #endregion

    #endregion
}