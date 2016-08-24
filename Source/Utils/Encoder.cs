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
using System.Globalization;
using System.Text;
using System.Web;

namespace DayPilot.Utils
{
    internal class Encoder
    {
        private static HttpApplication _app = null;
        private static HttpServerUtility Server
        {
            get
            {
                if (HttpContext.Current != null)
                    return HttpContext.Current.Server;

                if (_app == null)
                    _app = new HttpApplication();
                return _app.Server;
            }
        }

        internal static string HtmlEncode(string input)
        {
            if (input == null)
            {
                return null;
            }

            return Server.HtmlEncode(input);
        }


        internal static string UrlEncode(string input)
        {
//            return Server.UrlPathEncode(input);
            if (input == null)
            {
                return null;
            }
            return input.Replace("&", "%26");
//            return Server.UrlEncode(input); 
        }

        internal static string UrlDecode(string input)
        {
            return Server.UrlDecode(input);
        }

        internal static DateTime UrlDecodeDateTime(string input)
        {
            CultureInfo culture = new CultureInfo("en-US");
            string decoded = Server.UrlDecode(input);

            try
            {
                return DateTime.ParseExact(decoded, culture.DateTimeFormat.SortableDateTimePattern, culture.DateTimeFormat);
            }
            catch (FormatException e)
            {
                throw new ApplicationException("Unable to parse DateTime: '" + decoded + "'.", e);
            }
        }

        internal static string UrlEncode(IList list)
        {
            StringBuilder sb = new StringBuilder();

            bool isFirst = true;
            foreach (object o in list)
            {
                string item;
                if (o is DateTime)
                {
                    DateTime dt = (DateTime) o;
                    item = dt.ToString("s");
                }
                else if (o == null)
                {
                    item = String.Empty;
                }
                else
                {
                    item = o.ToString();
                }

                if (!isFirst)
                {
                    sb.Append("&");
                }

                sb.Append(UrlEncode(item));

                isFirst = false;
            }

            return sb.ToString();
        }
    }
}
