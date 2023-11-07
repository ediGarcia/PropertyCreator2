using System;

namespace PropertyCreator2.Models;

public class PopupButtonEventArgs : EventArgs
{
    #region Properties

    /// <summary>
    /// Gets or sets whether the current event should be cancelled.
    /// </summary>
    public bool Cancel { get; set; }

    #endregion
}