using System;
using System.Collections.Generic;
using System.Text;

namespace DayPilot.Web.Ui.Enums
{
    /// <summary>
    /// Enumeration of callback update types.
    /// </summary>
    public enum CallBackUpdateType
    {
        /// <summary>
        /// Don't refresh anything
        /// </summary>
        None,

        /// <summary>
        /// Refresh events collection only.
        /// </summary>
        EventsOnly,

        /// <summary>
        /// Full refresh.
        /// </summary>
        Full,

        /// <summary>
        /// Uses EventsOnly of Full, depending on detected changes.
        /// </summary>
        Auto
    }
}
