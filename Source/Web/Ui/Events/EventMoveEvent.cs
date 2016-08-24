using System;
using System.Collections.Generic;
using System.Text;
using DayPilot.Json;

namespace DayPilot.Web.Ui.Events
{
    /// <summary>
    /// Delegate for <see cref="DayPilotCalendar.EventMove">DayPilotCalendar.EventMove</see> event.
    /// </summary>
    public delegate void EventMoveEventHandler(object sender, EventMoveEventArgs e);

    /// <summary>
    /// Class that holds event arguments for <see cref="DayPilotCalendar.EventMove">DayPilotCalendar.EventMove</see> event.
    /// </summary>
    public class EventMoveEventArgs : DayPilotEventArgs
    {
        internal EventMoveEventArgs(JsonData parameters, JsonData data)
        {
            Id = (string)parameters["e"]["value"];
            OldStart = (DateTime)parameters["e"]["start"];
            OldEnd = (DateTime)parameters["e"]["end"];
            Text = (string)parameters["e"]["text"];
            Data = data;

            NewStart = (DateTime)parameters["newStart"];
            NewEnd = (DateTime)parameters["newEnd"];
        }

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
