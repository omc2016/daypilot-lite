using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace DayPilot.Utils
{
    internal class ViewStateHelper
    {
        internal static Hashtable ToHashtable(StateBag viewState)
        {
            Hashtable ht = new Hashtable();
            foreach (DictionaryEntry entry in viewState)
            {
                string s = (string)entry.Key;
                StateItem o = (StateItem)entry.Value;

                ht[s] = o.Value;
            }

            return ht;
        }

    }
}
