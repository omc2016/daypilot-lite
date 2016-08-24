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

namespace DayPilot.Web.Ui.Enums
{
    /// <summary>
    /// Enumeration of height specification types.
    /// </summary>
    public enum HeightSpecEnum
    {
        /// <summary>
        /// The height will be adjusted to full height. All hours will be visible. No scrollbar.
        /// </summary>
        Full,

        /// <summary>
        /// The height will be set according to business hours. The other hours will not be accessible (there is no scrollbar).
        /// </summary>
        BusinessHours,


        BusinessHoursNoScroll
    }
}
