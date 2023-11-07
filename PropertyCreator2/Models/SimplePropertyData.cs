using System;

namespace PropertyCreator2.Models;

[Serializable]
public class SimplePropertyData : PropertyData
{
    #region Properties

    /// <summary>
    /// Gets or sets whether the property should notify when its value is changed.
    /// </summary>
    public bool NotifyChanges
    {
        get => _notifyChanges;
        set
        {
            _notifyChanges = value;
            NotifyPropertyChange(nameof(NotifyChanges));
        }
    }

    /// <summary>
    /// Gets or sets whether the property method should use an explicit private variable.
    /// </summary>
    public bool UsePrivateVariable
    {
        get => _usePrivateVariable;
        set
        {
            _usePrivateVariable = value;
            NotifyPropertyChange(nameof(UsePrivateVariable));

            if (!UsePrivateVariable)
                NotifyChanges = false;
        }
    }

    #endregion

    private bool _notifyChanges;
    private bool _usePrivateVariable;

    #region Public Methods

    #region Clone
    /// <inheritdoc />
    public override SimplePropertyData Clone() =>
        new()
        {
            AccessModifier = AccessModifier,
            Category = Category,
            DefaultValue = DefaultValue,
            Description = Description,
            GetStatus = GetStatus,
            Name = Name,
            NotifyChanges = NotifyChanges,
            SetStatus = SetStatus,
            Type = Type,
            UseDescriptionAttribute = UseDescriptionAttribute,
            UsePrivateVariable = UsePrivateVariable
        };
    #endregion

    #region Set
    /// <inheritdoc />
    public override void Set(PropertyData propertyData)
    {
        AccessModifier = propertyData.AccessModifier;
        Category = propertyData.Category;
        DefaultValue = propertyData.DefaultValue;
        Description = propertyData.Description;
        GetStatus = propertyData.GetStatus;
        Name = propertyData.Name;
        SetStatus = propertyData.SetStatus;
        Type = propertyData.Type;
        UseDescriptionAttribute = propertyData.UseDescriptionAttribute;

        if (propertyData is SimplePropertyData simplePropertyData)
        {
            NotifyChanges = simplePropertyData.NotifyChanges;
            UsePrivateVariable = simplePropertyData.UsePrivateVariable;
        }
    } 
    #endregion

    #endregion
}