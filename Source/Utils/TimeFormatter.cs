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
using DayPilot.Web.Ui;
using DayPilot.Web.Ui.Enums;

namespace DayPilot.Utils
{
    /// <summary>
    /// Helper class for hour formatting.
    /// </summary>
    public class TimeFormatter
    {
        /// <summary>
        /// Extracts hour from DateTime class and formats it for 12/24 hours clock.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="clock"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GetHour(DateTime time, TimeFormat clock, string format)
        {
            return GetHour(time.Hour, clock, format);
        }

        /// <summary>
        /// Formats an hour number for 12/24 hours clock.
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="clock"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GetHour(int hour, TimeFormat clock, string format)
        {
            bool am = (hour / 12) == 0;
            if (clock == TimeFormat.Clock12Hours)
            {
                hour = hour % 12;
                if (hour == 0)
                    hour = 12;
            }

            string suffix = String.Empty;
            if (clock == TimeFormat.Clock12Hours)
            {
                if (am)
                {
                    suffix = "AM";
                }
                else
                {
                    suffix = "PM";
                }
            }

            if (String.IsNullOrEmpty(format))
            {
                format = "{0} {1}";
            }

            return String.Format(format, hour, suffix);

        }
    }
}
