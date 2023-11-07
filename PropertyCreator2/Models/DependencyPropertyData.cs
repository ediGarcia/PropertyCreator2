using System;
using System.ComponentModel;

namespace PropertyCreator2.Models;

[Serializable]
public class DependencyPropertyData : PropertyData
{
    #region Properties

    /// <summary>
    /// Gets or sets the type of the <see cref="DefaultValueAttribute"/> attribute.
    /// </summary>
    public string AttributeDefaultValueType
    {
        get => _attributeDefaultValueType;
        set
        {
            _attributeDefaultValueType = value;
            NotifyPropertyChange(nameof(AttributeDefaultValueType));
        }
    }

    /// <summary>
    /// Gets or sets the value of the <see cref="DefaultValueAttribute"/> attribute.
    /// </summary>
    public string AttributeDefaultValue
    {
        get => _attributeDefaultValue;
        set
        {
            _attributeDefaultValue = value;
            NotifyPropertyChange(nameof(AttributeDefaultValue));
        }
    }

    /// <summary>
    /// Gets or sets the owner class type.
    /// </summary>
    public string OwnerType
    {
        get => _ownerType;
        set
        {
            _ownerType = value;
            NotifyPropertyChange(nameof(OwnerType));
        }
    }

    #endregion

    private string _attributeDefaultValueType;
    private string _attributeDefaultValue;
    private string _ownerType = "UserControl";

    public DependencyPropertyData() =>
        UseDescriptionAttribute = true;

    #region Public Methods

    #region Clone
    /// <inheritdoc />
    public override DependencyPropertyData Clone() =>
        new()
        {
            AccessModifier = AccessModifier,
            AttributeDefaultValue = AttributeDefaultValue,
            AttributeDefaultValueType = AttributeDefaultValueType,
            Category = Category,
            DefaultValue = DefaultValue,
            Description = Description,
            GetStatus = GetStatus,
            Name = Name,
            OwnerType = OwnerType,
            SetStatus = SetStatus,
            Type = Type,
            UseDescriptionAttribute = UseDescriptionAttribute
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

        if (propertyData is DependencyPropertyData dependencyPropertyData)
        {
            AttributeDefaultValueType = dependencyPropertyData.AttributeDefaultValueType;
            AttributeDefaultValue = dependencyPropertyData.AttributeDefaultValue;
            OwnerType = dependencyPropertyData.OwnerType;
        }
    }
    #endregion

    #endregion
}