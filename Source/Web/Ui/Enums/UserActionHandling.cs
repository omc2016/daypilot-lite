using System;
using System.Collections.Generic;
using System.Text;

namespace DayPilot.Web.Ui.Enums
{
    /// <summary>
    /// UserActionHandling enumeration defines possible types of user event handling.
    /// </summary>
    /// <remarks>
    /// The user action can be handled in several ways:
    /// <list type="">
    /// <item>on the client-side by custom JavaScript code(JavaScript)</item>
    /// <item>on the server-side using a PostBack request (PostBack)</item>
    /// <item>on the server-side using an AJAX callback request (CallBack)</item>
    /// <item>it can be disabled at all (Disabled)</item>
    /// </list>
    /// </remarks>
    public enum UserActionHandling
    {
        /// <summary>
        /// The user action will run a JavaScript function.
        /// </summary>
        JavaScript,

        /// <summary>
        /// The user action will call a PostBack event.
        /// </summary>
        PostBack,

        /// <summary>
        /// The user action will call an AJAX CallBack event.
        /// </summary>
        CallBack,

        /// <summary>
        /// This functionality is disabled at all.
        /// </summary>
        Disabled
    }
}
