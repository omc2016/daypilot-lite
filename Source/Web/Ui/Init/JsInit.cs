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

using System.Drawing;
using System.Text;
using DayPilot.Web.Ui.Json;

namespace DayPilot.Web.Ui.Init
{
    internal class JsInit
    {
        protected StringBuilder sb = new StringBuilder();

        protected const string tempVar = "v";


        internal void appendSerialized(string property, object value)
        {
            appendProp(property, SimpleJsonSerializer.Serialize(value), false);
        }

        internal void appendProp(string property, object val, bool apo)
        {
            if (apo)
            {
                string escapedVal = null;
                if (val != null)
                {
                    escapedVal = SimpleJsonSerializer.EscapeString(val.ToString());
                }
                sb.AppendLine(tempVar + "." + property + " = '" + escapedVal + "';");
            }
            else
            {
                sb.AppendLine(tempVar + "." + property + " = " + val + ";");
            }
        }

        internal void appendProp(string property, object val)
        {
            if (val == null)
            {
                appendProp(property, "null", false);
            }
            else
            {
                appendProp(property, val, true);
            }
        }


        internal void appendProp(string property, Color val)
        {
            appendProp(property, ColorTranslator.ToHtml(val), true);
        }

        internal void appendProp(string property, bool value)
        {
            appendProp(property, value.ToString().ToLower(), false);
        }

        internal void appendProp(string property, int value)
        {
            appendProp(property, value, false);
        }

        internal void appendProp(string property, double value)
        {
            appendProp(property, value, false);
        }


    }
}
