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
    /// Delegate for <see cref="DayPilotCalendar.TimeRangeSelected">DayPilotCalendar.TimeRangeSelected</see> event.
    /// </summary>
    public delegate void TimeRangeSelectedEventHandler(object sender, TimeRangeSelectedEventArgs e);

    /// <summary>
    /// Class that holds event arguments for <see cref="DayPilotCalendar.TimeRangeSelected">DayPilotCalendar.TimeRangeSelected</see> event.
    /// </summary>
    public class TimeRangeSelectedEventArgs : DayPilotEventArgs
    {
        internal TimeRangeSelectedEventArgs(DateTime start, DateTime end, string resource)
        {
            this.Start = start;
            this.End = end;
            this.Resource = resource;
        }

        internal TimeRangeSelectedEventArgs(JsonData parameters, JsonData data)
        {
            this.Start = (DateTime)parameters["start"];
            this.End = (DateTime)parameters["end"];
            this.Resource = (string)parameters["resource"];
            this.Data = data;
        }

        /// <summary>
        /// Selection start date and time.
        /// </summary>
        public DateTime Start { get; private set; }

        /// <summary>
        /// Selection end date and time.
        /// </summary>
        public DateTime End { get; private set; }

        /// <summary>
        /// Selection resource id (Value).
        /// </summary>
        public string Resource { get; private set; }
    }

}
