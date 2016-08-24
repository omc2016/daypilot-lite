using System;
using System.Collections.Generic;
using System.Text;

namespace DayPilot.Web.Ui.Events
{
    /// <summary>
    /// Enumeration of event sources (PostBack/CallBack).
    /// </summary>
    public enum EventSource
    {
        /// <summary>
        /// Event was executed by a PostBack.
        /// </summary>
        PostBack,

        /// <summary>
        /// Event was executed by an AJAX callback.
        /// </summary>
        CallBack
    }


    public class EventSourceParser
    {
        public static EventSource Parse(string input)
        {
            switch (input.ToUpper())
            {
                case "POSTBACK":
                    return EventSource.PostBack;
                case "CALLBACK":
                    return EventSource.CallBack;
                default:
                    throw new Exception("Unrecognized EventSource value.");
            }
        }
    }
}
