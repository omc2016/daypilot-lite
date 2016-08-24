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
    /// UserActionHandling enumeration defines possible types of user event handling.
    /// </summary>
    /// <remarks>
    /// The user action can be handled in several ways:
    /// <list type="">
    /// <item>on the client-side by custom JavaScript code(JavaScript)</item>
    /// <item>on the server-side using a PostBack request (PostBack)</item>
    /// <item>it can be disabled at all (Disabled)</item>
    /// </list>
    /// </remarks>
    public enum TimeRangeSelectedHandling
    {
        /// <summary>
        /// The user action will run a JavaScript function.
        /// </summary>
        JavaScript,

        /// <summary>
        /// The user action will call a PostBack event.
        /// </summary>
        PostBack,

        /// <summary>
        /// The user action will call a CallBack event.
        /// </summary>
        CallBack,

        /// <summary>
        /// This functionality is disabled at all.
        /// </summary>
        Disabled,

    }
}
