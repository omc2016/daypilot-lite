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
using System.Collections.Generic;
using DayPilot.Web.Ui.Data;

namespace DayPilot.Web.Ui.Events.Scheduler
{
    /// <summary>
    /// Delegate for BeforeResHeaderRender event.
    /// </summary>
    public delegate void BeforeResHeaderRenderEventHandler(object sender, BeforeResHeaderRenderEventArgs e);

    /// <summary>
    /// Class that holds event arguments for BeforeResHeaderRender event.
    /// </summary>
    public class BeforeResHeaderRenderEventArgs : EventArgs
    {
        internal BeforeResHeaderRenderEventArgs()
        {
            Columns = new List<ResourceColumn>();
        }

        /// <summary>
        /// Get or set the column/row header HTML. Obsolete. Use .Html instead.
        /// </summary>
        [Obsolete("Use .Html instead.")]
        public string InnerHTML { get { return Html; } set { Html = value; }}

        /// <summary>
        /// Get or set the column/row header HTML.
        /// </summary>
        public string Html { get; set; }

        /// <summary>
        /// Get the column/row id (see <see cref="Column.Value">Column.Value</see>, <see cref="Resource.Value">Resource.Value</see>). Obsolete. Use .Id instead.
        /// </summary>
        [Obsolete("Use .Id instead.")]
        public string Value { get; internal set; }

        /// <summary>
        /// Get the column/row id (see <see cref="Column.Value">Column.Value</see>, <see cref="Resource.Value">Resource.Value</see>).
        /// </summary>
        public string Id { get; internal set; }

        /// <summary>
        /// Get the column/row name (see <see cref="Column.Name">Column.Name</see>, <see cref="Resource.Name">Resource.Name</see>).
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Additional header columns collection.
        /// </summary>
        public List<ResourceColumn> Columns { get; internal set; }

        /// <summary>
        /// DataSource element containing the source data for this resource (Gantt view).
        /// </summary>
        public DataItemWrapper DataItem { get; internal set; }

    }

}
