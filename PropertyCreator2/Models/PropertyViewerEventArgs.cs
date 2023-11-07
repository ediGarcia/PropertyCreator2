using System;

namespace PropertyCreator2.Models;

public class PropertyViewerEventArgs : EventArgs
{
    #region Properties

    /// <summary>
    /// Gets the current property data.
    /// </summary>
    public PropertyData CurrentPropertyData { get; }

    #endregion

    public PropertyViewerEventArgs(PropertyData currentPropertyData) =>
        CurrentPropertyData = currentPropertyData;
}