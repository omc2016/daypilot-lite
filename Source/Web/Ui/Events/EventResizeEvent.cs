using System;
using System.Collections.Generic;
using System.Text;
using DayPilot.Json;

namespace DayPilot.Web.Ui.Events
{
    /// <summary>
    /// Delegate for <see cref="DayPilotCalendar.EventResize">DayPilotCalendar.EventResize</see> event.
    /// </summary>
    public delegate void EventResizeEventHandler(object sender, EventResizeEventArgs e);

    /// <summary>
    /// Class that holds event arguments for <see cref="DayPilotCalendar.EventResize">DayPilotCalendar.EventResize</see> event.
    /// </summary>
    public class EventResizeEventArgs : DayPilotEventArgs
    {
        internal EventResizeEventArgs(JsonData parameters, JsonData data)
        {
            this.Id = (string)parameters["e"]["value"];
            this.OldStart = (DateTime)parameters["e"]["start"];
            this.OldEnd = (DateTime)parameters["e"]["end"];
            this.Text = (string)parameters["e"]["text"];
            this.Data = data;

            this.NewStart = (DateTime)parameters["newStart"];
            this.NewEnd = (DateTime)parameters["newEnd"];
        }

        ///<summary>
        /// Event value (<see cref="DayPilotCalendar.DataIdField">DayPilotCalendar.DataIdField</see> property).
        ///</summary>
        public string Id { get; private set; }

        ///<summary>
        /// Original event starting date and time (<see cref="DayPilotCalendar.DataStartField">DayPilotCalendar.DataStartField</see> property).
        ///</summary>
        public DateTime OldStart { get; private set; }

        ///<summary>
        /// Original event ending date and time (<see cref="DayPilotCalendar.DataEndField">DayPilotCalendar.DataEndField</see> property).
        ///</summary>
        public DateTime OldEnd { get; private set; }

        ///<summary>
        /// New event starting date and time.
        ///</summary>
        public DateTime NewStart { get; private set; }

        ///<summary>
        /// New event ending date and time.
        ///</summary>
        public DateTime NewEnd { get; private set; }

        /// <summary>
        /// Event text. (<see cref="DayPilotCalendar.DataTextField">DayPilotCalendar.DataTextField</see> property).
        /// </summary>
        public string Text { get; private set; }

    }

}
