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

using System.Threading;
using DayPilot.Web.Ui.Enums;

namespace DayPilot.Utils
{
    /// <summary>
    /// Helper class for hour manipulation and formatting.
    /// </summary>
    public class Hour
    {
        /// <summary>
        /// Detects the hour format for "Auto" TimeFormat value.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static TimeFormat DetectTimeFormat(TimeFormat input)
        {
            if (input == TimeFormat.Auto)
            {
                if (Thread.CurrentThread.CurrentCulture.DateTimeFormat.AMDesignator == "AM")
                {
                    return TimeFormat.Clock12Hours;
                }
                else
                {
                    return TimeFormat.Clock24Hours;
                }
            }
            else
            {
                return input;
            }
        }
    }
}
