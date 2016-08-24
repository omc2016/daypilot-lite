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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DayPilot.Web.Ui.Data;

namespace DayPilot.Web.Ui
{
    /// <summary>
    /// Day handles events of a single day.
    /// </summary>
    internal class Day : ISerializable
    {
        internal List<Event> events = new List<Event>();
        private List<Block> blocks = new List<Block>();

        internal int cellDuration; // in minutes

        private DateTime start;
        internal DateTime end;

        internal string Name;
        internal string Value;
        internal List<ResourceColumn> Columns = null;

        internal DataItemWrapper DataItem;

        internal DateTime Start
        {
            get { return start; }
        }

        internal DateTime End
        {
            get { return end; }
        }

        public Day(DateTime date)
        {
            this.start = date.Date;
            this.end = date.Date.AddDays(1);
        }

        internal Day(DateTime start, DateTime end, string header, string id, int cellDuration)
        {
            this.start = start.Date;
            this.end = end.Date;
            this.Name = header;
            this.Value = id;
            this.cellDuration = cellDuration;
        }

        private void stripAndAddEvent(Event e)
        {
            stripAndAddEvent(e.Start, e.End, e.Id, e.Text, e.Resource, e.Source);
        }

        private void stripAndAddEvent(DateTime start, DateTime end, string pk, string name, string resource, object source)
        {
            if (!String.IsNullOrEmpty(Value)) // this applies to resources view only
            {
                if (Value != resource) // don't add events that don't belong to this column
                    return;
            }

            // the event happens before this day
            if (end <= Start)
                return;

            // the event happens after this day
            if (start >= End)
                return;

            // this is invalid event that does have no duration
            if (start >= end)
                return;

            // fix the starting time
            if (start < Start)
                start = Start;


            // fix the ending time
            if (end > End)
                end = End;

            events.Add(new Event(pk, start, end, name, resource, source));
        }

/*
        private void stripAndAddEvent(Event e)
        {
            if (!String.IsNullOrEmpty(Value)) // this applies to resources view only
            {
                if (Value != e.Resource) // don't add events that don't belong to this column
                    return;
            }

            // the event happens before this day
            if (e.End <= Start)
                return;

            // the event happens after this day
            if (e.Start >= End.AddDays(1))
                return;

            // this is invalid event that has no duration
            if (e.Start >= e.End)
                return;

            //                Event part = new Event(this, e);
            events.Add(e);


        }
*/

        /// <summary>
        /// Loads events from ArrayList of Events.
        /// </summary>
        /// <param name="events">ArrayList that contains the Events.</param>
        public void Load(ArrayList events)
        {
            if (events == null)
            {
                return;
            }

            foreach (Event e in events)
            {
                stripAndAddEvent(e);
            }
            putIntoBlocks();
        }

        private void putIntoBlocks()
        {
            foreach (Event e in events)
            {
                // if there is no block, create the first one
                if (lastBlock == null)
                {
                    blocks.Add(new Block());
                }
                // or if the event doesn't overlap with the last block, create a new block
                else if (!lastBlock.OverlapsWith(e))
                {
                    blocks.Add(new Block());
                }

                // any case, add it to some block
                lastBlock.Add(e);

            }
        }

        private Block lastBlock
        {
            get
            {
                if (blocks.Count == 0)
                    return null;
                return blocks[blocks.Count - 1];
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }

        internal int MaxColumns()
        {
            int i = 1;
            foreach (Block b in blocks)
            {
                if (b.Columns.Count > i)
                    i = b.Columns.Count;
            }
            return i;
        }

        public DateTime BoxStart
        {
            get
            {
                DateTime min = DateTime.MaxValue;

                foreach (Block block in blocks)
                {
                    if (block.BoxStart < min)
                        min = block.BoxStart;
                }

                return min;
            }
        }

        /// <summary>
        /// The end of the box of the last event.
        /// </summary>
        public DateTime BoxEnd
        {
            get
            {
                DateTime max = DateTime.MinValue;

                foreach (Block block in blocks)
                {
                    if (block.BoxEnd > max)
                        max = block.BoxEnd;
                }

                return max;
            }
        }


    }
}
