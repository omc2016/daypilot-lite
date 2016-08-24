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

namespace DayPilot.Web.Ui
{
	/// <summary>
	/// Block is a set of concurrent events.
	/// </summary>
	internal class Block
	{
		internal ArrayList Columns;
		private ArrayList events = new ArrayList();


		internal Block()
		{
		}

		internal void Add(Event ev)
		{
			events.Add(ev);
			arrangeColumns();
		}

		private Column createColumn()
		{
			Column col = new Column();
			this.Columns.Add(col);
			col.Block = this;

			return col;
		}


		private void arrangeColumns()
		{
			// cleanup
			this.Columns = new ArrayList();

			foreach(Event e in events)
				e.Column = null;

			// there always will be at least one column because arrangeColumns is called only from Add()
			createColumn();

			foreach (Event e in events)
			{
				foreach (Column col in Columns)
				{
					if (col.CanAdd(e))
					{
						col.Add(e);
						break;
					}
				}
				// it wasn't placed 
				if (e.Column == null)
				{
					Column col = createColumn();
					col.Add(e);
				}
			}
		}

		internal bool OverlapsWith(Event e)
		{
			if (events.Count == 0)
				return false;

			return (this.BoxStart < e.BoxEnd && this.BoxEnd > e.BoxStart);
		}

		internal DateTime BoxStart
		{
			get
			{
				DateTime min = DateTime.MaxValue;

				foreach(Event ev in events)
				{
					if (ev.BoxStart < min)
						min = ev.BoxStart;
				}

				return min;
			}
		}
		
		internal DateTime BoxEnd
		{
			get
			{
				DateTime max = DateTime.MinValue;

				foreach(Event ev in events)
				{
					if (ev.BoxEnd > max)
						max = ev.BoxEnd;
				}

				return max;
			}
		}

	}
}
