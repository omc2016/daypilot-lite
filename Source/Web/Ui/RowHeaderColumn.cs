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

namespace DayPilot.Web.Ui
{
    /// <summary>
    /// Class representing a resource <see cref="DayPilotScheduler">DayPilotScheduler</see>.
    /// </summary>
    [Serializable]
    public class RowHeaderColumn
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public RowHeaderColumn()
        {
        }

        /// <summary>
        /// Constructor that sets the default values.
        /// </summary>
        /// <param name="title">Row name (visible).</param>
        /// <param name="val">Row value (id).</param>
        public RowHeaderColumn(string title, int width)
        {
            this.Title = title;
            this.Width = width;
        }

        /// <summary>
        /// Column width in pixels.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Get or set the column name.
        /// </summary>
        public string Title { get; set; }


        ///<summary>
        ///Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public override string ToString()
        {
            return Title;
        }
    }

}
