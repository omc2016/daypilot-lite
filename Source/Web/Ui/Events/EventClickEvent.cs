/*
Copyright © 2005 - 2016 Annpoint, s.r.o.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

-------------------------------------------------------------------------

NOTE: Reuse requires the following acknowledgement (see also NOTICE):
This product includes DayPilot (http://www.daypilot.org) developed by Annpoint, s.r.o.
*/


using System;
using DayPilot.Json;

namespace DayPilot.Web.Ui.Events
{
    /// <summary>
    /// Delegate for <see cref="DayPilotCalendar.EventClick">DayPilotCalendar.EventClick</see> event and <see cref="DayPilotScheduler.EventClick">DayPilotScheduler.EventClick</see>.
    /// </summary>
    public delegate void EventClickEventHandler(object sender, EventClickEventArgs e);


    /// <summary>
    /// Class that holds event arguments for <see cref="DayPilotCalendar.EventClick">DayPilotCalendar.EventClick</see> event and <see cref="DayPilotScheduler.EventClick">DayPilotScheduler.EventClick</see>.
    /// </summary>
    public class EventClickEventArgs : DayPilotEventArgs
    {

        internal EventClickEventArgs(string id)
        {
            Id = id;
        }

        internal EventClickEventArgs(JsonData parameters, JsonData data)
        {
            this.Id = (string)parameters["id"];
            this.Start = (DateTime)parameters["start"];
            this.End = (DateTime)parameters["end"];
            this.Text = (string)parameters["text"];
            this.Data = data;
        }

        ///<summary>
        /// Calendar event id (see <see cref="DayPilotCalendar.DataValueField">DayPilotCalendar.DataValueField</see> property and <see cref="DayPilotScheduler.DataValueField">DayPilotScheduler.DataValueField</see> property).
        ///</summary>
        [Obsolete("Use .Id instead.")]
        public string Value { get { return Id; } private set { Id = value; } }

        public string Id { get; private set; }

        /// <summary>
        /// Calendar event text (see <see cref="DayPilotCalendar.DataTextField"></see>).
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Calendar event start date (see <see cref="DayPilotCalendar.DataStartField"></see>).
        /// </summary>
        public DateTime Start { get; private set; }

        /// <summary>
        /// Calendar event end date (see <see cref="DayPilotCalendar.DataEndField"></see>).
        /// </summary>
        public DateTime End { get; private set; }

    }
}
