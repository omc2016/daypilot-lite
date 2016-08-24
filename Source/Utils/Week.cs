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
using System.Threading;

namespace DayPilot.Utils
{
    /// <summary>
    /// Helper class for week manipulation and formatting.
    /// </summary>
    public class Week
    {
        /// <summary>
        /// Gets the first day of this week. Based on current culture.
        /// </summary>
        /// <returns></returns>
        public static DateTime FirstDayOfWeek()
        {
            return FirstDayOfWeek(DateTime.Today);
        }

        /// <summary>
        /// Gets the first day of a week where day (parameter) belongs. Based on current culture.
        /// </summary>
        /// <returns></returns>
        public static DateTime FirstDayOfWeek(DateTime day)
        {
            return FirstDayOfWeek(day, Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek);
        }


        /// <summary>
        /// Gets the first day of a week where day (parameter) belongs. weekStart (parameter) specifies the starting day of week.
        /// </summary>
        /// <returns></returns> 
        public static DateTime FirstDayOfWeek(DateTime day, DayOfWeek weekStarts)
        {
            DateTime d = day;
            while (d.DayOfWeek != weekStarts)
            {
                d = d.AddDays(-1);
            }

            return d;
        }

        /// <summary>
        /// Returns Monday of the week where day (parameter) belongs.
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static DateTime FirstWorkingDayOfWeek(DateTime day)
        {
            return FirstDayOfWeek(day, DayOfWeek.Monday);
        }


        // http://konsulent.sandelien.no/VB_help/Week/
        // just for Monday being the first day of week
        /// <summary>
        /// Calculates week number for the specified date according to ISO 8601.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int WeekNrISO8601(DateTime date)
        {
            bool ThursdayFlag = false;
            int DayOfYear = date.DayOfYear;

            int StartWeekDayOfYear =
                 (int)(new DateTime(date.Year, 1, 1)).DayOfWeek;
            int EndWeekDayOfYear =
                 (int)(new DateTime(date.Year, 12, 31)).DayOfWeek;

            if (StartWeekDayOfYear == 0)
                StartWeekDayOfYear = 7;
            if (EndWeekDayOfYear == 0)
                EndWeekDayOfYear = 7;

            int DaysInFirstWeek = 8 - (StartWeekDayOfYear);

            if (StartWeekDayOfYear == 4 || EndWeekDayOfYear == 4)
                ThursdayFlag = true;

            int FullWeeks = (int)Math.Ceiling((DayOfYear - (DaysInFirstWeek)) / 7.0);

            int WeekNumber = FullWeeks;

            if (DaysInFirstWeek >= 4)
                WeekNumber = WeekNumber + 1;

            if (WeekNumber > 52 && !ThursdayFlag)
                WeekNumber = 1;

            if (WeekNumber == 0)
                WeekNumber = WeekNrISO8601(new DateTime(date.Year - 1, 12, 31));
            return WeekNumber;
        }


        /// <summary>
        /// Returns day names (using current culture).
        /// </summary>
        /// <returns></returns>
        public static ArrayList GetDayNames()
        {
            return GetDayNames("dddd");
        }

        /// <summary>
        /// Returns day names (using current culture).
        /// </summary>
        /// <param name="format">Corresponds to DateTime.ToString() formats. "DD" is also available (first two characters of short day name).</param>
        /// <returns></returns>
        public static ArrayList GetDayNames(string format)
        {
            ArrayList result = new ArrayList();
            DateTime start = new DateTime(2006, 12, 31); // Sunday
            for (int i = 0; i < 7; i++)
            {
                // this uses the current culture
                string name;
                if (format == "DD")
                {
                    name = start.AddDays(i).ToString("ddd").Substring(0, 2);
                }
                else
                {
                    name = start.AddDays(i).ToString(format);
                }


                result.Add(name);
            }
            return result;
        }

    }
}
