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
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;
using DayPilot.Utils;
using DayPilot.Web.Ui.Enums;
using DayPilot.Web.Ui.Init;
using DayPilot.Web.Ui.Json;

namespace DayPilot.Web.Ui
{
    internal class JsInitCalendar : JsInit
    {
        private readonly DayPilotCalendar _calendar;

        internal JsInitCalendar(DayPilotCalendar calendar)
        {
            this._calendar = calendar;
        }

        internal string GetCode()
        {
            this.sb = new StringBuilder();

            string scrollPos = "null";
            
            if (_calendar.HeightSpec != HeightSpecEnum.Full)
            {
                if (_calendar.ScrollPos == -1)
                {
                    scrollPos = Convert.ToString(_calendar.CellHeight*(_calendar.ScrollPositionHour)*2);
                }
                else
                {
                    scrollPos = _calendar.ScrollPos.ToString();
                }
            }

            List<Hashtable> events = _calendar.GetEvents();

            sb.AppendLine("<script type='text/javascript'>");
            sb.AppendLine(String.Format("/* DayPilotLite: {0} */", Assembly.GetExecutingAssembly().FullName));

            sb.AppendLine("function " + _calendar.ClientObjectName + "_Init() {");
            sb.AppendLine("var " + tempVar + " = new DayPilot.Calendar('" + _calendar.ClientID + "');");

            appendProp("api", 1);
            appendProp("businessBeginsHour", _calendar.BusinessBeginsHour);
            appendProp("businessEndsHour", _calendar.BusinessEndsHour);
            appendProp("clientName", _calendar.ClientObjectName);
            appendProp("cellHeight", _calendar.CellHeight);
            appendProp("columnMarginRight", _calendar.ColumnMarginRight);
            appendProp("theme", _calendar.Theme);
            appendProp("days", _calendar.Days);

            #if (DEBUG)
            appendProp("debuggingEnabled", true);
            #endif

            appendProp("durationBarVisible", _calendar.DurationBarVisible);
            appendProp("headerDateFormat", _calendar.ResolvedHeaderDateFormat);
            appendProp("headerHeight", _calendar.HeaderHeight);
            appendProp("height", _calendar.Height);
            appendProp("heightSpec", _calendar.HeightSpec);
            appendProp("hourWidth", _calendar.HourWidth);
            appendProp("initScrollPos", scrollPos);
            appendProp("showToolTip", _calendar.ShowToolTip);
            appendProp("startDate", _calendar.StartDate.ToString("s"), true);
            appendProp("timeFormat", Hour.DetectTimeFormat(_calendar.TimeFormat).ToString(), true);
            appendProp("uniqueID", _calendar.UniqueID);
            appendProp("viewType", _calendar.ViewType.ToString());
            appendProp("widthUnit", GetWidthUnitType());
            if (_calendar.Width != Unit.Empty)
            {
                appendProp("width", _calendar.Width, true);
            }

            // ensure WebForm_DoCallBack is available
            _calendar.Page.ClientScript.GetCallbackEventReference(_calendar, null, null, null, null, true);

            appendProp("eventClickHandling", _calendar.EventClickHandling, true);
            appendProp("onEventClick", "function(e) {" + _calendar.EventClickJavaScript + "}", false);

            appendProp("eventResizeHandling", _calendar.EventResizeHandling, true);
            appendProp("onEventResize", "function(e, newStart, newEnd) { " + _calendar.EventResizeJavaScript + "}", false);

            appendProp("eventMoveHandling", _calendar.EventMoveHandling, true);
            appendProp("onEventMove", "function(e, newStart, newEnd, newResource, external, ctrl, shift) { var newColumn = newResource; var oldColumn = e.resource(); " + _calendar.EventMoveJavaScript + "}", false);

            appendProp("timeRangeSelectedHandling", _calendar.TimeRangeSelectedHandling, true);
            appendProp("onTimeRangeSelected", "function(start, end, column) { var resource = column; " + _calendar.TimeRangeSelectedJavaScript + "}", false);

//            if (!String.IsNullOrEmpty(_calendar.CallBackErrorJavaScript))
//            {
//                appendProp("callbackError", "function(result, context) { " + _calendar.CallBackErrorJavaScript + " }", false);
//            }

            appendProp("events.list", SimpleJsonSerializer.Serialize(events), false);

            Hashtable hashes = new Hashtable();
            hashes["callBack"] = _calendar.CallBack.GetHash();
            hashes["events"] = _calendar.Hash(events);
            appendSerialized("hashes", hashes);


            sb.AppendLine(tempVar + ".init();");

            sb.AppendLine("return " + tempVar + ".initialized ? " + tempVar + " : null;");
            sb.AppendLine("}");

            sb.AppendLine("var " + _calendar.ClientObjectName + " = " + _calendar.ClientObjectName + "_Init() || " + _calendar.ClientObjectName + ";");

            sb.AppendLine("</script>");

            return sb.ToString();
        }

        private string GetWidthUnitType()
        {
            if (_calendar.Width == Unit.Empty)
            {
                return "Percentage";
            }
            return _calendar.Width.Type.ToString();

        }


    }
}
