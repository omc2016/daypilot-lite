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
using System.Text;
using DayPilot.Json;

namespace DayPilot.Web.Ui.Json
{
    /// <summary>
    /// Class for serializing simple objects to JSON. Supports null, int, double, bool, DateTime, string, IDictionary, and IList.
    /// </summary>
    public class SimpleJsonSerializer
    {
        private readonly StringBuilder _sb = new StringBuilder();

        /// <summary>
        /// Serializes an object to a JSON string. Unknown classes are serialized using ToString().
        /// </summary>
        /// <param name="obj">Supports null, int, double, bool, DateTime, string, IDictionary, and IList.</param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            SimpleJsonSerializer main = new SimpleJsonSerializer();
            main.SerializeObject(obj);
            return main._sb.ToString();
        }

        private void SerializeObject(object obj)
        {
            if (obj == null) // null
            {
                _sb.Append("null");
                return;
            }

            if (obj is int) // integer number
            {
                _sb.Append(obj);
                return;
            }

            if (obj is long) // integer number
            {
                _sb.Append(obj);
                return;
            }

            if (obj is double)
            {
                _sb.Append(obj);
                return;
            }

            if (obj is bool)
            {
                _sb.Append(obj.ToString().ToLower());
                return;
            }

            if (obj is DateTime)
            {
                DateTime dt = (DateTime) obj;
                Serialize(dt.ToString("s"));
                return;
            }

            if (obj is string)
            {
                Serialize(obj as string);
                return;
            }

            if (obj is JsonData)
            {
                Serialize(obj as JsonData);
                return;
            }

            if (obj is IDictionary)
            {
                Serialize(obj as IDictionary);
                return;
            }

            if (obj is IList)
            {
                Serialize(obj as IList);
                return;
            }

            // all other object serialized using ToString() (or throw an exception?)
            Serialize(obj.ToString());

        }

        private void Serialize(JsonData data)
        {
            switch(data.GetJsonType())
            {
                case JsonType.Array:
                    Serialize(data as IList);
                    return;
                case JsonType.Object:
                    Serialize(data as IDictionary);
                    return;
                case JsonType.Boolean:
                    SerializeObject((bool)data);
                    return;
                case JsonType.Double:
                    SerializeObject((double)data);
                    return;
                case JsonType.Int:
                    SerializeObject((int)data);
                    return;
                case JsonType.Long:
                    SerializeObject((long)data);
                    return;
                case JsonType.String:
                    Serialize((string)data);
                    return;
                case JsonType.None:
                    SerializeObject(null);
                    return;
            }

        }

        private void Serialize(IDictionary dict)
        {
            _sb.Append("{");

            bool first = true;
            foreach (string key in dict.Keys)
            {
                if (!first)
                {
                    _sb.Append(",");
                }

                Serialize(key);
                _sb.Append(":");
                SerializeObject(dict[key]);
                first = false;
            }

            _sb.Append("}");

        }

        private void Serialize(string str)
        {
            if (str == null)
            {
                _sb.Append("null");
                return;
            }
            _sb.Append("\"");
            _sb.Append(EscapeString(str));
            _sb.Append("\"");
        }

        // t n r f b 
        public static string EscapeString(string str)
        {
            return
                str.Replace("\\", "\\\\")
                    .Replace("\t", "\\t")
                    .Replace("\n", "\\n")
                    .Replace("\r", "\\r")
                    .Replace("\f", "\\f")
                    .Replace("\b", "\\b")
                    .Replace("\'", "\\'")
                    .Replace("\"", "\\\"")
                    .Replace("/", "\\/");
        }

        private void Serialize(IList list)
        {
            bool first = true;

            _sb.Append("[");
            foreach (object o in list)
            {
                if (!first)
                {
                    _sb.Append(",");
                }

                SerializeObject(o);

                first = false;
            }
            _sb.Append("]");
        }
    }
}