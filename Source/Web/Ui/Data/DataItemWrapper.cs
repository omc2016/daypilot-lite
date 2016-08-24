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
using System.Data;
using System.Reflection;
using System.Text;
using System.Web.UI;

namespace DayPilot.Web.Ui.Data
{
    public class DataItemWrapper
    {
        /// Gets the original DataItem object.
        public object Source { get; private set; }

        public DataItemWrapper(object dataItem)
        {
            Source = dataItem;
        }

        /// <summary>
        /// Gets a property value of the original DataItem object.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public object this[string property]
        {
            get
            {
                if (Source == null)
                {
                    throw new NullReferenceException("DataItem doesn't hold any source object.");
                }
                return ReadPropertyValue(Source, property);
            }
        }

        private static object ReadPropertyValue(object o, string property)
        {
            Type type = o.GetType();
            PropertyInfo pi = type.GetProperty(property);
            if (pi != null)
            {
                return pi.GetValue(o, null);
            }
            else // try to read using indexed property
            {
                MethodInfo mi = type.GetMethod("get_Item", new Type[] { typeof(String) });
                if (mi != null)
                {
                    return mi.Invoke(o, new object[] { property });
                }
            }

            throw new ArgumentException("Property or index not found.");
        }
    }
}
