using System;
using System.Collections.Generic;
using System.Text;
using DayPilot.Json;

namespace DayPilot.Web.Ui.Events
{
    /// <summary>
    /// Base EventArgs that allow to specify event source (PostBack or CallBack).
    /// </summary>
    public class DayPilotEventArgs : EventArgs
    {
        /// <summary>
        /// Event source: PostBack or CallBack.
        /// </summary>
        public EventSource Source { get; internal set; }


        /// <summary>
        /// Custom data.
        /// </summary>
        public JsonData Data { get; internal set; }
    }
}
