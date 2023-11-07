using System;
using System.ComponentModel;

namespace PropertyCreator2.Models;

[Serializable]
public class Settings
{
    #region Properties

    /// <summary>
    /// Gets or sets the last used dependency property owner type.
    /// </summary>
    public string LastUsedOwnerType { get; set; } = "UserControl";

    /// <summary>
    /// Gets the registered properties' data.
    /// </summary>
    public BindingList<PropertyData> Properties { get; } = new();

    #endregion
}