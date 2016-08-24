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
using DayPilot.Web.Ui;

namespace DayPilot.Web.Ui
{
	/// <summary>
	/// Column is a column of events in a Block.
	/// </summary>
	public class Column
	{
		private ArrayList events = new ArrayList();
		internal Block Block;

		/// <summary>
		/// Gets the width of the column in percent.
		/// </summary>
		public int WidthPct
		{
			get
			{
				if (Block == null)
					throw new ApplicationException("This Column does not belong to any Block.");

				if (Block.Columns.Count == 0)
					throw new ApplicationException("Internal error: Problem with Block.Column.Counts (it is zero).");

				// the last block will be a bit longer to make sure the total width is 100%
				if (isLastInBlock)
					return 100 / Block.Columns.Count + 100 % Block.Columns.Count;
				else
					return 100 / Block.Columns.Count;
			}
		}

		/// <summary>
		/// Gets the starting percent of the column.
		/// </summary>
		public int StartsAtPct
		{
			get
			{
				if (Block == null)
					throw new ApplicationException("This Column does not belong to any Block.");

				if (Block.Columns.Count == 0)
					throw new ApplicationException("Internal error: Problem with Block.Column.Counts (it is zero).");

				return 100 / Block.Columns.Count * Number;
			}
		}

		private bool isLastInBlock
		{
			get
			{
				return Block.Columns[Block.Columns.Count - 1] == this;
			}
		}

		internal Column()
		{
		}

		internal bool CanAdd(Event e)
		{
			foreach (Event ev in events)
			{
				if (ev.OverlapsWith(e))
					return false;
			}
			return true;
		}

		internal void Add(Event e)
		{
			if (e.Column != null)
				throw new ApplicationException("This Event was already placed into a Column.");

			events.Add(e);
			e.Column = this;
		}

		/// <summary>
		/// Gets the order number of the column.
		/// </summary>
		public int Number
		{
			get
			{
				if (Block == null)
					throw new ApplicationException("This Column doesn't belong to any Block.");

				return Block.Columns.IndexOf(this);
			}
		}
	}
}
