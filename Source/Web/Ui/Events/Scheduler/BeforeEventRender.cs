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
using DayPilot.Web.Ui;
using DayPilot.Web.Ui.Data;
using DayPilot.Web.Ui.Enums;

namespace DayPilot.Web.Ui.Events.Scheduler
{

    /// <summary>
    /// Delegate for <see cref="DayPilotCalendar.BeforeEventRender">DayPilotCalendar.BeforeEventRender</see> event.
    /// </summary>
    public delegate void BeforeEventRenderEventHandler(object sender, BeforeEventRenderEventArgs e);

    /// <summary>
    /// Class that holds event arguments for <see cref="DayPilotCalendar.BeforeEventRender">DayPilotCalendar.BeforeEventRender</see> event.
    /// </summary>
    public class BeforeEventRenderEventArgs : EventArgs
    {

        internal BeforeEventRenderEventArgs(Event e)
        {
            this.Value = e.Id;
            this.Start = e.Start;
            this.End = e.End;
            this.Text = e.Text;
            this.ResourceId = e.Resource;
            DataItem = new DataItemWrapper(e.Source);

        }

        /// <summary>
        /// Color of the bar on the left side of an event.
        /// </summary>
        public string DurationBarColor { get; set; }

        /// <summary>
        /// Color of the event background.
        /// </summary>
        public string BackgroundColor { get; set; }

        /// <summary>
        /// Inner HTML of the event (use it to rewrite the default content).
        /// </summary>
        [Obsolete("Use Html property instead.")]
        public string InnerHTML { get { return Html; } set { Html = value; } }

        public string Html { get; set; }

        /// <summary>
        /// Event id (<see cref="DayPilotScheduler.DataIdField">DayPilotScheduler.DataIdField</see> property). Read-only. Obsolete - use Id property instead.
        /// </summary>
        [Obsolete("Use Id property instead.")]
        public string Value { get { return Id; } internal set { Id = value; } }


        /// <summary>
        /// Event id (<see cref="DayPilotScheduler.DataIdField">DayPilotScheduler.DataIdField</see> property). Read-only.
        /// </summary>
        public string Id { get; internal set; }


        /// <summary>
        /// Event starting date and time (<see cref="DayPilotScheduler.DataStartField">DayPilotScheduler.DataStartField</see> property). Read-only.
        /// </summary>
        public DateTime Start { get; internal set; }

        /// <summary>
        /// Event ending date and time (<see cref="DayPilotScheduler.DataStartField">DayPilotScheduler.DataStartField</see> property). Read-only.
        /// </summary>
        public DateTime End { get; internal set; }

        /// <summary>
        /// Event resource id. Read-only.
        /// </summary>
        public string ResourceId { get; internal set; }

        [Obsolete("Please use ResourceId instead.")]
        public string ColumnId
        {
            get { return ResourceId; }
        }

        /// <summary>
        /// CSS class for the event. 
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// Event text (<see cref="DayPilotScheduler.DataTextField">DayPilotScheduler.DataTextField</see> property). Read-only.
        /// </summary>
        public string Text { get; internal set; }

        /// <summary>
        /// Tooltip that appears when user hovers over the event (title HTML tag).
        /// </summary>
        public string ToolTip { get; set; }

        /// <summary>
        /// Set to false to disable clicking on this calendar event on the client-side.
        /// </summary>
        /// <remarks>
        /// The clicking will be enabled only if these two conditions are met: <see cref="DayPilotCalendar.EventClickHandling">DayPilotCalendar.EventClickHandling</see> is not set to Disabled and EventClickEnabled is set to true.
        /// </remarks>
        public bool EventClickEnabled { get; set; }

        /// <summary>
        /// DataSource element containing the source data for this event.
        /// </summary>
        public DataItemWrapper DataItem { get; private set; }
    }

}
