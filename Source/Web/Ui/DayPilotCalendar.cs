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
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DayPilot.Json;
using DayPilot.Utils;
using DayPilot.Web.Ui.Ajax;
using DayPilot.Web.Ui.Design;
using DayPilot.Web.Ui.Enums;
using DayPilot.Web.Ui.Enums.Calendar;
using DayPilot.Web.Ui.Events;
using DayPilot.Web.Ui.Events.Calendar;
using DayPilot.Web.Ui.Json;
using Calendar = System.Web.UI.WebControls.Calendar;
using CommandEventArgs = DayPilot.Web.Ui.Events.CommandEventArgs;
using CommandEventHandler = DayPilot.Web.Ui.Events.CommandEventHandler;

namespace DayPilot.Web.Ui
{
    /// <summary>
    /// DayPilot is a component for showing a day schedule.
    /// </summary>
    [Themeable(true)]
    [ToolboxBitmap(typeof(Calendar))]
    [Designer(typeof(DayPilotCalendarDesigner))]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public partial class DayPilotCalendar : DataBoundControl, IPostBackEventHandler, ICallbackEventHandler
    {
        private Day[] _days;

        private string _dataStartField;
        private string _dataEndField;
        private string _dataTextField;
        private string _dataIdField;

        // day header
        private bool _showHeader = true;
        private int _headerHeight = 21;
        private string _headerDateFormat = "d";

        private ArrayList Items;

        internal int ScrollPos = -1;

        private List<Hashtable> _columns;

        // callback
        private Exception _callbackException;
        private Hashtable _hashes = new Hashtable();
        private CallBackUpdateType _callbackUpdateType = CallBackUpdateType.None;
        private DayPilotCalendarCallBack _callback;
        private bool _databindCalled;
  
        /// <summary>
        /// Event called when the user clicks an event in the calendar. It's only called when EventClickHandling is set to PostBack.
        /// </summary>
        [Category("User actions")]
        [Description("Event called when the user clicks an event in the calendar.")]
        public event EventClickEventHandler EventClick;

        /// <summary>
        /// This event is fired when a user resizes an event by top or bottom border dragging.
        /// </summary>
        /// <remarks>
        /// When user starts resizing the event, a shadow outlining the new size appears. When the mouse button is released, this event is fired on the server.<br/>
        /// In the custom event handler you should decide whether the action is allowed. If so, update the persistent data storage (such as a database), call <see cref="BaseDataBoundControl.DataBind">DataBind()</see> and <see cref="Update()">Update()</see>.
        /// After calling <see cref="Update()">Update()</see> the events will be updated on the client-side.
        /// </remarks>
        [Category("User actions")]
        [Description("Fires when a user resizes an event.  EventResizeHandling must be set to PostBack or CallBack.")]
        public event EventResizeEventHandler EventResize;

        /// <summary>
        /// This event is fired when a user moves an event by dragging.
        /// </summary>
        /// <remarks>
        /// When user starts moving the event, a shadow outlining the new position appears. When the mouse button is released, this event is fired on the server.<br/>
        /// In the custom event handler you should decide whether the action is allowed. If so, update the persistent data storage (such as a database), call <see cref="BaseDataBoundControl.DataBind">DataBind()</see> and <see cref="Update()">Update()</see>.
        /// After calling <see cref="Update()">Update()</see> the events will be updated on the client-side.
        /// </remarks>
        [Category("User actions")]
        [Description("Fires when a user moves an event. EventMoveHandling must be set to PostBack or CallBack.")]
        public event EventMoveEventHandler EventMove;

        /// <summary>
        /// This event is fired using client-side .commandCallBack() function. Use it to refresh the control using fast callback.
        /// </summary>
        [Category("User actions")]
        [Description("This event is fired using client-side .commandCallBack() function.")]
        public event CommandEventHandler Command;

        /// <summary>
        /// Event called when the user clicks a free space in the calendar. It's only called when TimeRangeSelectedHandling is set to PostBack.
        /// </summary>
        [Category("User actions")]
        [Description("Event called when the user clicks a free space in the calendar.")]
        public event TimeRangeSelectedEventHandler TimeRangeSelected;


        /// <summary>
        /// Use this event to modify event properties before rendering.
        /// </summary>
        [Category("Preprocessing")]
        [Description("Use this event to modify event properties before rendering.")]
        public event BeforeEventRenderEventHandler BeforeEventRender;


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ScriptManagerHelper.RegisterClientScriptInclude(this, typeof(DayPilotCalendar), "Common.js", Page.ClientScript.GetWebResourceUrl(typeof(DayPilotCalendar), "DayPilot.Resources.Common.js"));
            ScriptManagerHelper.RegisterClientScriptInclude(this, typeof(DayPilotCalendar), "Calendar.js", Page.ClientScript.GetWebResourceUrl(typeof(DayPilotCalendar), "DayPilot.Resources.Calendar.js"));

        }


        #region Viewstate

        /// <summary>
        /// Loads ViewState.
        /// </summary>
        /// <param name="savedState"></param>
        protected override void LoadViewState(object savedState)
        {
            if (savedState == null)
                return;

            object[] vs = (object[])savedState;

            if (vs.Length != 2)
            {
                throw new ArgumentException("Wrong savedState object.");
            }

            if (vs[0] != null)
            {
                base.LoadViewState(vs[0]);
            }

            if (vs[1] != null)
            {
                Items = (ArrayList)vs[1];
            }

        }

        /// <summary>
        /// Saves ViewState.
        /// </summary>
        /// <returns></returns>
        protected override object SaveViewState()
        {
            object[] vs = new object[2];
            vs[0] = base.SaveViewState();
            vs[1] = Items;

            return vs;
        }

        #endregion

        #region PostBack


        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventArgument"></param>
        public void RaisePostBackEvent(string eventArgument)
        {
            string action = eventArgument.Substring(0, 4);
            if (action == "JSON")
            {
                ExecuteEventJson(eventArgument.Substring(4));
            }
            else
            {
                throw new Exception("Unsupported PostBack format.");
            }
        }


        private void DoTimeRangeSelected(TimeRangeSelectedEventArgs e)
        {
            if (TimeRangeSelected != null)
            {
                TimeRangeSelected(this, e);
            }
        }

        private void DoEventClick(EventClickEventArgs e)
        {
            if (EventClick != null)
            {
                EventClick(this, e);
            }
        }

        #endregion

        #region Rendering

        /// <summary>
        /// Renders the component HTML code.
        /// </summary>
        /// <param name="output"></param>
        protected override void Render(HtmlTextWriter output)
        {
            if (DesignMode)
            {
                RenderDesignMode(output);
                return;

            }
            RenderClientSide(output);

            // create the main javascript object for the control
            JsInitCalendar jsi = new JsInitCalendar(this);
            ScriptManagerHelper.RegisterStartupScript(this, typeof(DayPilotCalendar), ClientID + "object", jsi.GetCode(), false);

        }

        private void RenderClientSide(HtmlTextWriter output)
        {
            output.AddAttribute("id", ClientID);
            output.RenderBeginTag("div");
            output.RenderEndTag();
        }

        private void RenderDesignMode(HtmlTextWriter output)
        {
            LoadEventsToDays();

            // <div>
            if (CssOnly)
            {
                output.AddAttribute("class", PrefixCssClass("_main"));
            }
            output.AddAttribute("width", Width.ToString());
            output.AddStyleAttribute("position", "relative");
            output.RenderBeginTag("div");

            // <table>
            output.AddAttribute("id", ClientID);
            output.AddAttribute("cellpadding", "0");
            output.AddAttribute("cellspacing", "0");
            output.AddAttribute("border", "0");
            if (!CssOnly)
            {
                output.AddStyleAttribute("border-bottom", "1px solid " + ColorTranslator.ToHtml(BorderColor));
                output.AddStyleAttribute("text-align", "left");
            }
            output.RenderBeginTag("table");

            // <tr>
            output.RenderBeginTag("tr");

            // <td>
            output.AddAttribute("valign", "top");
            output.RenderBeginTag("td");

            if (ShowHours)
            {
                RenderHourNamesTable(output);
            }

            // </td>
            output.RenderEndTag();

            // <td>
            output.AddAttribute("width", "100%");
            output.AddAttribute("valign", "top");
            output.RenderBeginTag("td");

            output.AddStyleAttribute("position", "relative");
            output.RenderBeginTag("div");

            RenderEventsAndCells(output);

            output.RenderEndTag();

            // </td>
            output.RenderEndTag();
            // </tr>
            output.RenderEndTag();
            // </table>
            output.RenderEndTag();

            // </div>
            output.RenderEndTag();
            
        }


        private void RenderHourNamesTable(HtmlTextWriter output)
        {
            output.AddAttribute("cellpadding", "0");
            output.AddAttribute("cellspacing", "0");
            output.AddAttribute("border", "0");
            output.AddAttribute("width", HourWidth.ToString());

            if (!CssOnly)
            {
                output.AddStyleAttribute("border-left", "1px solid " + ColorTranslator.ToHtml(BorderColor));
            }
            output.RenderBeginTag("table");

            if (!CssOnly)
            {
                // <tr> first emtpy
                output.AddStyleAttribute("height", "1px");
                if (!CssOnly)
                {
                    output.AddStyleAttribute("background-color", ColorTranslator.ToHtml(BorderColor));
                }
                output.RenderBeginTag("tr");

                output.RenderBeginTag("td");
                output.RenderEndTag();

                // </tr> first empty
                output.RenderEndTag();
            }

            if (ShowHeader)
            {
                RenderHourHeader(output);
            }

            for (DateTime i = VisibleStartTime; i < VisibleEndTime; i = i.AddHours(1))
            {
                RenderHourTr(output, i);
            }

            // </table>
            output.RenderEndTag();

        }

        private string PrefixCssClass(string name)
        {
            if (String.IsNullOrEmpty(Theme))
            {
                return String.Empty;
            }
            return Theme + name;
        }


        private void RenderHourTr(HtmlTextWriter output, DateTime i)
        {

            // <tr>
            output.AddStyleAttribute("height", (CellHeight*2) + "px");
            output.RenderBeginTag("tr");

            // <td>
            output.AddAttribute("valign", "bottom");
            if (!CssOnly)
            {
                output.AddStyleAttribute("background-color", ColorTranslator.ToHtml(HourNameBackColor));
                output.AddStyleAttribute("cursor", "default");
            }
            output.RenderBeginTag("td");

            // <div> block
            output.AddStyleAttribute("display", "block");
            output.AddStyleAttribute("height", ((CellHeight * 2) - 1) + "px");
            if (!CssOnly)
            {
                output.AddStyleAttribute("border-bottom", "1px solid " + ColorTranslator.ToHtml(HourNameBorderColor));
                output.AddStyleAttribute("text-align", "right");
            }
            else
            {
                output.AddStyleAttribute("position", "relative");
                output.AddStyleAttribute("height", CellHeight * 2 + "px");
                output.AddAttribute("class", PrefixCssClass("_rowheader"));
            }
            output.RenderBeginTag("div");

            // <div> text
            if (!CssOnly)
            {
                output.AddStyleAttribute("padding", "2px");
                output.AddStyleAttribute("font-family", HourFontFamily);
                output.AddStyleAttribute("font-size", HourFontSize);
            }
            else
            {
                output.AddAttribute("class", PrefixCssClass("_rowheader_inner"));
            }
            output.RenderBeginTag("div");

            int hour = i.Hour;
            bool am = (i.Hour / 12) == 0;
            if (TimeFormat == TimeFormat.Clock12Hours)
            {
                hour = i.Hour % 12;
                if (hour == 0)
                {
                    hour = 12;
                }
            }

            output.Write(hour);
            if (!CssOnly)
            {
                output.AddStyleAttribute("font-size", "10px");
                output.AddStyleAttribute("vertical-align", "super");
                output.RenderBeginTag("span");
                output.Write("&nbsp;");
                //output.Write("<span style='font-size:10px; vertical-align: super; '>&nbsp;");
            }
            else
            {
                output.AddAttribute("class", PrefixCssClass("_rowheader_minutes"));
                output.RenderBeginTag("span");
                //output.Write("<span>");
            }
            if (TimeFormat == TimeFormat.Clock24Hours)
            {
                output.Write("00");
            }
            else
            {
                output.Write(am ? "AM" : "PM");
            }
            output.RenderEndTag();
//            output.Write("</span>");

            output.RenderEndTag();
            output.RenderEndTag();
            output.RenderEndTag(); // </td>
            output.RenderEndTag(); // </tr>
        }


        private void RenderHourHeader(HtmlTextWriter output)
        {

            // <tr>
            output.AddStyleAttribute("height", (HeaderHeight) + "px");
            output.RenderBeginTag("tr");

            // <td>
            output.AddAttribute("valign", "bottom");
            if (!CssOnly)
            {
                output.AddStyleAttribute("background-color", ColorTranslator.ToHtml(HourNameBackColor));
                output.AddStyleAttribute("cursor", "default");
            }
            output.RenderBeginTag("td");

            // <div> block
            output.AddStyleAttribute("display", "block");
            if (!CssOnly)
            {
                output.AddStyleAttribute("border-bottom", "1px solid " + ColorTranslator.ToHtml(BorderColor));
                output.AddStyleAttribute("text-align", "right");
            }
            else
            {
                output.AddStyleAttribute("position", "relative");
                output.AddStyleAttribute("height", HeaderHeight + "px");
                output.AddAttribute("class", PrefixCssClass("_corner"));
            }
            output.RenderBeginTag("div");

            // <div> text
            if (!CssOnly)
            {
                output.AddStyleAttribute("padding", "2px");
                output.AddStyleAttribute("font-size", "6pt");
            }
            else
            {
                output.AddAttribute("class", PrefixCssClass("_corner_inner"));
            }
            output.RenderBeginTag("div");

            output.Write("&nbsp;");

            output.RenderEndTag();
            output.RenderEndTag();
            output.RenderEndTag(); // </td>
            output.RenderEndTag(); // </tr>
        }

        private void RenderEventsAndCells(HtmlTextWriter output)
        {
            RenderCellsTable(output);
            RenderEventsTable(output);
        }

        private void RenderCellsTable(HtmlTextWriter output)
        {
            // <table>
            output.AddAttribute("cellpadding", "0");
            output.AddAttribute("cellspacing", "0");
            output.AddAttribute("border", "0");
            output.AddAttribute("width", "100%");
            
            output.AddStyleAttribute("table-layout", "fixed");
            output.AddStyleAttribute("position", "absolute");
            output.AddStyleAttribute("left", "0px");
            output.AddStyleAttribute("top", "0px");
            if (!CssOnly)
            {
                output.AddStyleAttribute("border-left", "1px solid " + ColorTranslator.ToHtml(BorderColor));
            }
            output.RenderBeginTag("table");


            // header
            if (ShowHeader)
            {
                RenderDayHeaders(output);
            }

            output.WriteLine("<!-- empty cells -->");

            // render all cells

            for (DateTime i = VisibleStartTime; i < VisibleEndTime; i = i.AddHours(1))
            {

                // <tr> first half-hour
                output.RenderBeginTag("tr");

                AddHalfHourCells(output, i, true, false);

                // </tr>
                output.RenderEndTag();

                // <tr> second half-hour
                output.AddStyleAttribute("height", CellHeight + "px");
                output.RenderBeginTag("tr");

                bool isLastRow = (i == VisibleEndTime.AddHours(-1));
                AddHalfHourCells(output, i, false, isLastRow);

                // </tr>
                output.RenderEndTag();
            }

            // </table>
            output.RenderEndTag();
            
        }

        private void RenderEventsTable(HtmlTextWriter output)
        {
            // <table>
            output.AddAttribute("cellpadding", "0");
            output.AddAttribute("cellspacing", "0");
            output.AddAttribute("border", "0");
            output.AddAttribute("width", "100%");

            output.AddStyleAttribute("table-layout", "fixed");
            output.AddStyleAttribute("position", "absolute");
            output.AddStyleAttribute("left", "0px");
            output.AddStyleAttribute("top", "0px");

            if (!CssOnly)
            {
                output.AddStyleAttribute("border-left", "1px solid " + ColorTranslator.ToHtml(BorderColor));
            }
            output.RenderBeginTag("table");

            // <tr> first
            output.AddStyleAttribute("height", "1px");
            if (!CssOnly)
            {
                output.AddStyleAttribute("background-color", ColorTranslator.ToHtml(BorderColor));
            }
            output.RenderBeginTag("tr");

            RenderEventTds(output);

            // </tr> first
            output.RenderEndTag();

            // </table>
            output.RenderEndTag();

        }

        private void RenderDayHeaders(HtmlTextWriter output)
        {
            if (!CssOnly)
            {
                output.AddStyleAttribute("background-color", ColorTranslator.ToHtml(HourNameBackColor));
            }
            output.AddStyleAttribute("height", HeaderHeight + "px");
            output.RenderBeginTag("tr");

            foreach (Day d in _days)
            {
                DateTime h = new DateTime(d.Start.Year, d.Start.Month, d.Start.Day, 0, 0, 0);

                // <td>
                output.AddAttribute("valign", "bottom");
                if (!CssOnly)
                {
                    output.AddStyleAttribute("background-color", ColorTranslator.ToHtml(HourNameBackColor));
                    output.AddStyleAttribute("cursor", "default");
                    output.AddStyleAttribute("border-right", "1px solid " + ColorTranslator.ToHtml(BorderColor));
                }
                output.RenderBeginTag("td");

                // <div> block
                //output.AddStyleAttribute("display", "block");
                output.AddStyleAttribute("height", HeaderHeight + "px");
                if (!CssOnly)
                {
                    output.AddStyleAttribute("border-bottom", "1px solid " + ColorTranslator.ToHtml(BorderColor));
                    output.AddStyleAttribute("text-align", "center");
                }
                else
                {
                    output.AddStyleAttribute("position", "relative");
                    output.AddAttribute("class", PrefixCssClass("_colheader"));
                }
                output.RenderBeginTag("div");

                // <div> text
                if (!CssOnly)
                {
                    output.AddStyleAttribute("padding", "2px");
                    output.AddStyleAttribute("font-family", DayFontFamily);
                    output.AddStyleAttribute("font-size", DayFontSize);
                }
                else
                {
                    output.AddAttribute("class", PrefixCssClass("_colheader_inner"));
                }
                output.RenderBeginTag("div");

                output.Write(h.ToString(_headerDateFormat));

                output.RenderEndTag();
                output.RenderEndTag();
                output.RenderEndTag(); // </td>


            }

            output.RenderEndTag();
        }



        private void RenderEventTds(HtmlTextWriter output)
        {

            int dayPctWidth = 100 / _days.Length;

            for (int i = 0; i < _days.Length; i++)
            {
                Day d = _days[i];


                // <td>
                output.AddStyleAttribute("height", "1px");
                if (!CssOnly)
                {
                    output.AddStyleAttribute("text-align", "left");
                }
                output.AddAttribute("width", dayPctWidth + "%");
                output.RenderBeginTag("td");

                // <div> position
                output.AddStyleAttribute("display", "block");
                output.AddStyleAttribute("margin-right", ColumnMarginRight + "px"); 
                output.AddStyleAttribute("position", "relative");
                output.AddStyleAttribute("height", "1px");
                if (!CssOnly)
                {
                    output.AddStyleAttribute("font-size", "1px");
                }
                output.AddStyleAttribute("margin-top", "-1px");
                output.RenderBeginTag("div");

                foreach (Event e in d.events)
                {
                    RenderEvent(output, e, d);
                }

                // </div> position
//                output.Write(divPosition.EndTag());
                output.RenderEndTag();

                // </td>
                output.RenderEndTag();
            }
        }


        private BeforeEventRenderEventArgs GetEva(Event e)
        {
            string displayText = e.Text;
            if (ShowEventStartEnd)
            {
                displayText = e.Text + " (" + e.Start.ToShortTimeString() + " - " + e.End.ToShortTimeString() + ")";
            }          

            BeforeEventRenderEventArgs ea = new BeforeEventRenderEventArgs(e);
            ea.InnerHTML = displayText;
            ea.ToolTip = displayText;
            ea.EventClickEnabled = EventClickHandling != EventClickHandlingEnum.Disabled;
            if (!CssOnly)
            {
                ea.DurationBarColor = ColorTranslator.ToHtml(DurationBarColor);
                ea.BackgroundColor = ColorTranslator.ToHtml(EventBackColor);
            }

            DoBeforeEventRender(ea);

            return ea;
        }

        private void DoBeforeEventRender(BeforeEventRenderEventArgs args)
        {
            if (BeforeEventRender != null)
            {
                BeforeEventRender(this, args);
            }
        }

        private void RenderEvent(HtmlTextWriter output, Event e, Day d)
        {

            string displayText = e.Text + " (" + e.Start.ToShortTimeString() + " - " + e.End.ToShortTimeString() + ")";

            BeforeEventRenderEventArgs ea = new BeforeEventRenderEventArgs(e);
            ea.Html = displayText;
            ea.ToolTip = displayText;
            ea.EventClickEnabled = EventClickHandling != EventClickHandlingEnum.Disabled;
            if (!CssOnly)
            {
                ea.DurationBarColor = ColorTranslator.ToHtml(DurationBarColor);
                ea.BackgroundColor = ColorTranslator.ToHtml(EventBackColor);
            }

            DoBeforeEventRender(ea);

            // real box dimensions and position
            DateTime dayVisibleStart = new DateTime(d.Start.Year, d.Start.Month, d.Start.Day, VisibleStartTime.Hour, 0, 0);
            DateTime realBoxStart = e.BoxStart < dayVisibleStart ? dayVisibleStart : e.BoxStart;

            DateTime dayVisibleEnd;
            if (VisibleEndTime.Day == 1)
            {
                dayVisibleEnd = new DateTime(d.Start.Year, d.Start.Month, d.Start.Day, VisibleEndTime.Hour, 0, 0);
            }
            else if (VisibleEndTime.Day == 2)
            {
                dayVisibleEnd = new DateTime(d.Start.Year, d.Start.Month, d.Start.Day, VisibleEndTime.Hour, 0, 0).AddDays(1);
            }
            else
            {
                throw new Exception("Unexpected time for dayVisibleEnd.");
            }

            DateTime realBoxEnd = e.BoxEnd > dayVisibleEnd ? dayVisibleEnd : e.BoxEnd;

            // top
            double top = (realBoxStart - dayVisibleStart).TotalHours * (CellHeight * 2) + 1;
            if (ShowHeader)
            {
                top += HeaderHeight;
            }

            // height
            double height = (realBoxEnd - realBoxStart).TotalHours * (CellHeight * 2) - 2;
            int startDelta = (int)Math.Floor((e.Start - realBoxStart).TotalHours * (CellHeight * 2));
            int endDelta = (int)Math.Floor((realBoxEnd - e.End).TotalHours * (CellHeight * 2));

            double barHeight = height - startDelta - endDelta;
            int barTop = startDelta;

            // It's outside of visible area (for NonBusinessHours set to Hide).
            // Don't draw it in that case.
            if (height <= 0)
            {
                return;
            }

            if (CssOnly)
            {
                output.AddStyleAttribute("-moz-user-select", "none"); // prevent text selection in FF
                output.AddStyleAttribute("-khtml-user-select", "none"); // prevent text selection
                output.AddStyleAttribute("-webkit-user-select", "none"); // prevent text selection
                output.AddStyleAttribute("user-select", "none"); // prevent text selection
                output.AddAttribute("unselectable", "on");
                output.AddStyleAttribute("position", "absolute");
                output.AddStyleAttribute("left", e.Column.StartsAtPct + "%");
                output.AddStyleAttribute("top", top + "px");
                output.AddStyleAttribute("width", e.Column.WidthPct + "%");
                output.AddStyleAttribute("height", (realBoxEnd - realBoxStart).TotalHours * (CellHeight * 2) + "px");

                string cssClass = PrefixCssClass("_event");
                if (!String.IsNullOrEmpty(ea.CssClass))
                {
                    cssClass += " " + ea.CssClass;
                }

                output.AddAttribute("class", cssClass);


                if (ea.EventClickEnabled && EventClickHandling != EventClickHandlingEnum.Disabled)
                {
                    if (EventClickHandling == EventClickHandlingEnum.PostBack)
                    {
                        output.AddAttribute("onclick", "javascript:event.cancelBubble=true;" + Page.ClientScript.GetPostBackEventReference(this, "PK:" + e.Id));
                    }
                    else
                    {
                        output.AddAttribute("onclick", "javascript:event.cancelBubble=true;" + String.Format(EventClickJavaScript, e.Id));
                    }
                }

                output.AddAttribute("onmouseover", "this.className+=' " + PrefixCssClass("_event_hover") + "';event.cancelBubble=true;");
                output.AddAttribute("onmouseout", "if (this.className) { this.className = this.className.replace(' " + PrefixCssClass("_event_hover") + "', ''); } ;event.cancelBubble=true;");

                output.RenderBeginTag("div");

                // inner
                output.AddAttribute("class", PrefixCssClass("_event_inner"));
                output.AddAttribute("unselectable", "on");

                if (!String.IsNullOrEmpty(ea.BackgroundColor))
                {
                    output.AddStyleAttribute("background", ea.BackgroundColor);
                }
                output.RenderBeginTag("div");
                output.Write(ea.InnerHTML);
                output.RenderEndTag();

                // bar
                output.AddAttribute("class", PrefixCssClass("_event_bar"));
                output.AddStyleAttribute("position", "absolute");
                output.RenderBeginTag("div");

                double barTopPct = (100.0*barTop/height);
                double barHeightPct = (100.0*barHeight/height);

                if (barTopPct + barHeightPct > 100)
                {
                    barHeightPct = 100 - barTopPct;
                }

                // bar_inner
                output.AddAttribute("class", PrefixCssClass("_event_bar_inner"));
                output.AddStyleAttribute("top", barTopPct + "%");
                output.AddStyleAttribute("height", barHeightPct + "%");
                if (!String.IsNullOrEmpty(ea.DurationBarColor))
                {
                    output.AddStyleAttribute("background-color", ea.DurationBarColor);
                }
                output.RenderBeginTag("div");

                // bar_inner
                output.RenderEndTag();

                // bar                
                output.RenderEndTag();


                output.RenderEndTag();
            }
            else
            {
                // MAIN BOX
                output.AddAttribute("onselectstart", "return false;"); // prevent text selection in IE

                if (ea.EventClickEnabled && EventClickHandling != EventClickHandlingEnum.Disabled)
                {
                    if (EventClickHandling == EventClickHandlingEnum.PostBack)
                    {
                        output.AddAttribute("onclick", "javascript:event.cancelBubble=true;" + Page.ClientScript.GetPostBackEventReference(this, "PK:" + e.Id));
                    }
                    else
                    {
                        output.AddAttribute("onclick", "javascript:event.cancelBubble=true;" + String.Format(EventClickJavaScript, e.Id));
                    }

                    output.AddStyleAttribute("cursor", "pointer");

                }

                output.AddStyleAttribute("-moz-user-select", "none"); // prevent text selection in FF
                output.AddStyleAttribute("-khtml-user-select", "none"); // prevent text selection
                output.AddStyleAttribute("user-select", "none"); // prevent text selection
                output.AddStyleAttribute("position", "absolute");
                if (!CssOnly)
                {
                    output.AddStyleAttribute("font-family", EventFontFamily);
                    output.AddStyleAttribute("font-size", EventFontSize);
                    output.AddStyleAttribute("white-space", "no-wrap");
                    output.AddStyleAttribute("background-color", ColorTranslator.ToHtml(EventBorderColor));
                }
                output.AddStyleAttribute("left", e.Column.StartsAtPct + "%");
                output.AddStyleAttribute("top", top + "px");
                output.AddStyleAttribute("width", e.Column.WidthPct + "%");
                output.AddStyleAttribute("height", (realBoxEnd - realBoxStart).TotalHours * (CellHeight * 2) + "px");
                output.RenderBeginTag("div");

                // FIX BOX - to fix the outer/inner box differences in Mozilla/IE (to create border)

                if (ea.EventClickEnabled && EventClickHandling != EventClickHandlingEnum.Disabled)
                {
                    if (!CssOnly)
                    {
                        output.AddAttribute("onmouseover", "this.style.backgroundColor='" + ColorTranslator.ToHtml(EventHoverColor) + "';event.cancelBubble=true;");
                        output.AddAttribute("onmouseout", "this.style.backgroundColor='" + ea.BackgroundColor + "';event.cancelBubble=true;");
                    }
                }

                if (ShowToolTip)
                {
                    output.AddAttribute("title", ea.ToolTip);
                }

                output.AddStyleAttribute("margin-top", "1px");
                output.AddStyleAttribute("display", "block");
                output.AddStyleAttribute("height", height + "px");
                if (!CssOnly)
                {
                    output.AddStyleAttribute("background-color", ea.BackgroundColor);
                    output.AddStyleAttribute("border-left", "1px solid " + ColorTranslator.ToHtml(EventBorderColor));
                    output.AddStyleAttribute("border-right", "1px solid " + ColorTranslator.ToHtml(EventBorderColor));
                }
                output.AddStyleAttribute("overflow", "hidden");
                output.RenderBeginTag("div");

                // blue column
                if (e.Start > realBoxStart)
                {

                }

                output.AddStyleAttribute("float", "left");
                output.AddStyleAttribute("width", "5px");
                output.AddStyleAttribute("height", height - startDelta - endDelta + "px");
                output.AddStyleAttribute("margin-top", startDelta + "px");
                if (!CssOnly)
                {
                    output.AddStyleAttribute("background-color", ea.DurationBarColor);
                    output.AddStyleAttribute("font-size", "1px");
                }
                output.RenderBeginTag("div");
                output.RenderEndTag();

                // right border of blue column
                output.AddStyleAttribute("float", "left");
                output.AddStyleAttribute("width", "1px");
                if (!CssOnly)
                {
                    output.AddStyleAttribute("background-color", ColorTranslator.ToHtml(EventBorderColor));
                }
                output.AddStyleAttribute("height", "100%");
                output.RenderBeginTag("div");
                output.RenderEndTag();

                // space
                output.AddStyleAttribute("float", "left");
                output.AddStyleAttribute("width", "2px");
                output.AddStyleAttribute("height", "100%");
                output.RenderBeginTag("div");
                output.RenderEndTag();

                // PADDING BOX
                output.AddStyleAttribute("padding", "1px");
                output.RenderBeginTag("div");

                output.Write(ea.InnerHTML);

                // closing the PADDING BOX
                output.RenderEndTag();

                // closing the FIX BOX
                output.RenderEndTag();

                // closing the MAIN BOX
                output.RenderEndTag();
                
            }

        }

        private void AddHalfHourCells(HtmlTextWriter output, DateTime hour, bool hourStartsHere, bool isLast)
        {
            foreach (Day d in _days)
            {
                DateTime h = new DateTime(d.Start.Year, d.Start.Month, d.Start.Day, hour.Hour, 0, 0);
                AddHalfHourCell(output, h, hourStartsHere, isLast);
            }
        }

        private void AddHalfHourCell(HtmlTextWriter output, DateTime hour, bool hourStartsHere, bool isLast)
        {
            bool isBusiness = true;
            if (hour.Hour < BusinessBeginsHour || hour.Hour >= BusinessEndsHour || hour.DayOfWeek == DayOfWeek.Saturday || hour.DayOfWeek == DayOfWeek.Sunday)
            {
                isBusiness = false;
            }


            string cellBgColor = isBusiness ? ColorTranslator.ToHtml(BackColor) : ColorTranslator.ToHtml(NonBusinessBackColor);
            string borderBottomColor = ColorTranslator.ToHtml(hourStartsHere ? HourHalfBorderColor : HourBorderColor);

            DateTime startingTime = hour;
            if (!hourStartsHere)
            {
                startingTime = hour.AddMinutes(30);
            }

            if (TimeRangeSelectedHandling != TimeRangeSelectedHandling.Disabled)
            {
                if (TimeRangeSelectedHandling == TimeRangeSelectedHandling.PostBack)
                {
                    output.AddAttribute("onclick", "javascript:" + Page.ClientScript.GetPostBackEventReference(this, "TIME:" + startingTime.ToString("s")));
                }
                else
                {
                    output.AddAttribute("onclick", "javascript:" + String.Format(TimeRangeSelectedJavaScript, startingTime.ToString("s")));
                }

                if (!CssOnly)
                {
                    output.AddAttribute("onmouseover", "this.style.backgroundColor='" + ColorTranslator.ToHtml(HoverColor) + "';");
                    output.AddAttribute("onmouseout", "this.style.backgroundColor='" + cellBgColor + "';");
                }

                output.AddStyleAttribute("cursor", "pointer");

            }
            else
            {
                output.AddStyleAttribute("cursor", "default");
            }

            output.AddAttribute("valign", "bottom");
            if (!CssOnly)
            {
                output.AddStyleAttribute("background-color", cellBgColor);
                output.AddStyleAttribute("border-right", "1px solid " + ColorTranslator.ToHtml(BorderColor));
            }
            else
            {
                output.AddStyleAttribute("position", "relative");
            }
            output.AddStyleAttribute("cursor", "hand");
            output.AddStyleAttribute("height", CellHeight + "px");
            output.RenderBeginTag("td");

            if (!CssOnly)
            {
                // FIX BOX - to fix the outer/inner box differences in Mozilla/IE (to create border)
                output.AddStyleAttribute("display", "block");
                output.AddStyleAttribute("height", "14px");
                if (!isLast)
                {
                    output.AddStyleAttribute("border-bottom", "1px solid " + borderBottomColor);
                }
            }
            else
            {
                output.AddStyleAttribute("position", "relative");
                output.AddStyleAttribute("height", CellHeight + "px");

                string className = PrefixCssClass("_cell");
                if (isBusiness)
                {
                    className += " " + PrefixCssClass("_cell_business");
                }
                output.AddAttribute("class", className);


            }
            output.RenderBeginTag("div");

            if (!CssOnly)
            {
                // required
                output.Write("<span style='font-size:1px'>&nbsp;</span>");
            }
            else
            {
                output.AddAttribute("class", PrefixCssClass("_cell_inner"));
                output.RenderBeginTag("div");
                output.RenderEndTag();
            }

            // closing the FIX BOX
            output.RenderEndTag();

            // </td>
            output.RenderEndTag();

        }


        #endregion

        #region Calculations


        /// <summary>
        /// This is only a relative time. The date part should be ignored.
        /// </summary>
        private DateTime VisibleStartTime
        {
            get
            {

                DateTime date = new DateTime(1900, 1, 1);

                //if (NonBusinessHours == NonBusinessHoursBehavior.Show)
                if (HeightSpec == HeightSpecEnum.Full && !HideFreeCells)
                {
                    return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                }

                DateTime start = new DateTime(date.Year, date.Month, date.Day, BusinessBeginsHour, 0, 0);

                /*
                if (HeightSpec == HeightSpecEnum.BusinessHoursNoScroll)
                {
                    return start;
                }
                 */

                if (_days == null)
                    return start;

                if (TotalEvents == 0)
                    return start;

                foreach (Day d in _days)
                {
                    DateTime boxStart = new DateTime(date.Year, date.Month, date.Day, d.BoxStart.Hour, d.BoxStart.Minute, d.BoxStart.Second);
                    if (boxStart < start)
                        start = boxStart;
                }

                return new DateTime(start.Year, start.Month, start.Day, start.Hour, 0, 0);


            }
        }

        /// <summary>
        /// This is only a relative time. The date part should be ignored.
        /// </summary>
        private DateTime VisibleEndTime
        {
            get
            {
                DateTime date = new DateTime(1900, 1, 1);

                //if (NonBusinessHours == NonBusinessHoursBehavior.Show)
                if (HeightSpec == HeightSpecEnum.Full && !HideFreeCells)
                {
                    return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0).AddDays(1);
                }

                DateTime end;
                if (BusinessEndsHour == 24)
                {
                    end = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0).AddDays(1);
                }
                else
                {
                    end = new DateTime(date.Year, date.Month, date.Day, BusinessEndsHour, 0, 0);
                }

                /*
                if (HeightSpec == HeightSpecEnum.BusinessHoursNoScroll)
                    return end;
                 */ 

                if (_days == null)
                    return end;

                if (TotalEvents == 0)
                    return end;

                foreach (Day d in _days)
                {

                    bool addDay = false;
                    if (d.BoxEnd > DateTime.MinValue && d.BoxEnd.AddDays(-1) >= d.Start)
                        addDay = true;

                    DateTime boxEnd = new DateTime(date.Year, date.Month, date.Day, d.BoxEnd.Hour, d.BoxEnd.Minute, d.BoxEnd.Second);

                    if (addDay)
                        boxEnd = boxEnd.AddDays(1);

                    if (boxEnd > end)
                        end = boxEnd;
                }

                if (end.Minute != 0)
                    end = end.AddHours(1);

                return new DateTime(end.Year, end.Month, end.Day, end.Hour, 0, 0);
            }
        }


        private int TotalEvents
        {
            get
            {
                int ti = 0;
                foreach (Day d in _days)
                    ti += d.events.Count;

                return ti;
            }
        }
        #endregion

        #region Data binding


        protected override void PerformSelect()
        {
            // Call OnDataBinding here if bound to a data source using the
            // DataSource property (instead of a DataSourceID), because the
            // databinding statement is evaluated before the call to GetData.       
            if (!IsBoundUsingDataSourceID)
            {
                OnDataBinding(EventArgs.Empty);
            }

            // The GetData method retrieves the DataSourceView object from  
            // the IDataSource associated with the data-bound control.            
            GetData().Select(CreateDataSourceSelectArguments(), OnDataSourceViewSelectCallback);

            // The PerformDataBinding method has completed.
            RequiresDataBinding = false;
            MarkAsDataBound();

            // Raise the DataBound event.
            OnDataBound(EventArgs.Empty);
        }

        private void OnDataSourceViewSelectCallback(IEnumerable retrievedData)
        {
            // Call OnDataBinding only if it has not already been 
            // called in the PerformSelect method.
            if (IsBoundUsingDataSourceID)
            {
                OnDataBinding(EventArgs.Empty);
            }
            // The PerformDataBinding method binds the data in the  
            // retrievedData collection to elements of the data-bound control.
            PerformDataBinding(retrievedData);
        }

        protected override void PerformDataBinding(IEnumerable retrievedData)
        {
            // don't load events in design mode
            if (DesignMode)
            {
                return;
            }

            _databindCalled = true;

            base.PerformDataBinding(retrievedData);

            if (String.IsNullOrEmpty(DataStartField))
                throw new NullReferenceException("DataStartField property must be specified.");

            if (String.IsNullOrEmpty(DataEndField))
                throw new NullReferenceException("DataEndField property must be specified.");

            if (String.IsNullOrEmpty(DataTextField))
                throw new NullReferenceException("DataTextField property must be specified.");

            if (String.IsNullOrEmpty(DataValueField))
                throw new NullReferenceException("DataValueField property must be specified.");


            // Verify data exists.
            if (retrievedData != null)
            {
                Items = new ArrayList();

                foreach (object dataItem in retrievedData)
                {

                    DateTime start = Convert.ToDateTime(DataBinder.GetPropertyValue(dataItem, DataStartField, null));
                    DateTime end = Convert.ToDateTime(DataBinder.GetPropertyValue(dataItem, DataEndField, null));
                    string name = Convert.ToString(DataBinder.GetPropertyValue(dataItem, DataTextField, null));
                    string pk = Convert.ToString(DataBinder.GetPropertyValue(dataItem, DataIdField, null));

                    var ev = new Event(pk, start, end, name);
                    ev.Source = dataItem;
                    Items.Add(ev);

                }

                Items.Sort(new EventComparer());

            }
        }

        private void LoadEventsToDays()
        {

            if (EndDate < StartDate)
            {
                throw new ArgumentException("EndDate must be equal to or greater than StartDate.");
            }

            int dayCount = (int)(EndDate - StartDate).TotalDays + 1;
            _days = new Day[dayCount];

            for (int i = 0; i < _days.Length; i++)
            {
                _days[i] = new Day(StartDate.AddDays(i));

                if (Items != null)
                {
                    _days[i].Load(Items);
                }
            }
        }

        internal List<Hashtable> GetEvents()
        {
            List<Hashtable> events = new List<Hashtable>();

            if (Items != null)
            {
                foreach (Event e in Items)
                {
                    events.Add(GetEventMap(e));
                }
            }

            return events;
        }


        internal Hashtable GetEventMap(Event e)
        {
            BeforeEventRenderEventArgs eva = GetEva(e);

            Hashtable se = new Hashtable();

            se["id"] = e.Id;
            se["text"] = e.Text;
            se["start"] = e.Start.ToString("s");
            se["end"] = e.End.ToString("s");

            if (eva.Html != e.Text)
            {
                se["html"] = eva.Html;
            }
            if (eva.ToolTip != e.Text)
            {
                se["toolTip"] = eva.ToolTip;
            }
            if (CssOnly)
            {
                if (!String.IsNullOrEmpty(eva.BackgroundColor))
                {
                    se["backColor"] = eva.BackgroundColor;
                }
            }
            else
            {
                if (eva.BackgroundColor != ColorTranslator.ToHtml(EventBackColor))
                {
                    se["backColor"] = eva.BackgroundColor;
                }
            }

            if (eva.DurationBarColor != ColorTranslator.ToHtml(DurationBarColor))
            {
                se["barColor"] = eva.DurationBarColor;
            }

            if (!String.IsNullOrEmpty(eva.DurationBarBackColor))
            {
                se["barBackColor"] = eva.DurationBarBackColor;
            }

            if (!String.IsNullOrEmpty(eva.CssClass))
            {
                se["cssClass"] = eva.CssClass;
            }

            if (!eva.EventClickEnabled)
            {
                se["clickDisabled"] = true;
            }
            return se;
        }


        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the start of the business day (in hours).
        /// </summary>
        [Description("Start of the business day (hour from 0 to 23).")]
        [Category("Appearance")]
        [DefaultValue(9)]
        public int BusinessBeginsHour
        {
            get
            {
                if (ViewState["BusinessBeginsHour"] == null)
                    return 9;
                return (int)ViewState["BusinessBeginsHour"];
            }
            set
            {
                if (value < 0)
                    ViewState["BusinessBeginsHour"] = 0;
                else if (value > 23)
                    ViewState["BusinessBeginsHour"] = 23;
                else
                    ViewState["BusinessBeginsHour"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the end of the business day (hours).
        /// </summary>
        [Description("End of the business day (hour from 1 to 24).")]
        [Category("Appearance")]
        [DefaultValue(18)]
        public int BusinessEndsHour
        {
            get
            {
                if (ViewState["BusinessEndsHour"] == null)
                    return 18;
                return (int)ViewState["BusinessEndsHour"];
            }
            set
            {
                if (value < BusinessBeginsHour)
                    ViewState["BusinessEndsHour"] = BusinessBeginsHour + 1;
                else if (value > 24)
                    ViewState["BusinessEndsHour"] = 24;
                else
                    ViewState["BusinessEndsHour"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the height of the time cells in pixels.
        /// </summary>
        [Description("Height of the time cells in pixels.")]
        [Category("Layout")]
        [DefaultValue(20)]
        public int CellHeight
        {
            get
            {
                if (ViewState["CellHeight"] == null)
                    return 20;
                return (int)ViewState["CellHeight"];
            }
            set
            {
                ViewState["CellHeight"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the width of the hour cell in pixels.
        /// </summary>
        [Description("Width of the hour cell in pixels.")]
        [Category("Layout")]
        [DefaultValue(50)]
        public int HourWidth
        {
            get
            {
                if (ViewState["HourWidth"] == null)
                    return 50;
                return (int)ViewState["HourWidth"];
            }
            set
            {
                ViewState["HourWidth"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Javascript code that is executed when the users clicks on an event. '{0}' will be replaced by the primary key of the event.
        /// </summary>
        [Description("Javascript code that is executed when the users clicks on an event. '{0}' will be replaced by the primary key of the event.")]
        [Category("User actions")]
        [DefaultValue("alert('{0}');")]
        public string EventClickJavaScript
        {
            get
            {
                if (ViewState["EventClickJavaScript"] == null)
                    return "alert('{0}');";
                return (string)ViewState["EventClickJavaScript"];
            }
            set
            {
                ViewState["EventClickJavaScript"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Javascript code that is executed when the users clicks on a free time slot. '{0}' will be replaced by the starting time of that slot (i.e. '9:00'.
        /// </summary>
        [Description("Javascript code that is executed when the users clicks on a free time slot. '{0}' will be replaced by the starting time of that slot (i.e. '9:00'.")]
        [Category("User actions")]
        [DefaultValue("alert(start + ' ' + end);")]
        public string TimeRangeSelectedJavaScript
        {
            get
            {
                if (ViewState["TimeRangeSelectedJavaScript"] == null)
                    return "alert(start + ' ' + end);";
                return (string)ViewState["TimeRangeSelectedJavaScript"];
            }
            set
            {
                ViewState["TimeRangeSelectedJavaScript"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the first day to be shown. Default is DateTime.Today.
        /// </summary>
        [Description("The first day to be shown. Default is DateTime.Today.")]
        [Category("Behavior")]
        public DateTime StartDate
        {
            get
            {
                DateTime start;
                if (ViewState["StartDate"] == null)
                {
                    start = DateTime.Today;
                }
                else
                {
                    start = (DateTime)ViewState["StartDate"];

                }


                switch (ViewType)
                {
                    case ViewTypeEnum.WorkWeek:
                        return Week.FirstWorkingDayOfWeek(start);
                    case ViewTypeEnum.Week:
                        return Week.FirstDayOfWeek(start);
                }

                return start;

            }
            set
            {
                ViewState["StartDate"] = new DateTime(value.Year, value.Month, value.Day);
            }
        }

        /// <summary>
        /// Gets the last day to be shown.
        /// </summary>
        public DateTime EndDate
        {
            get
            {
                return StartDate.AddDays(Days - 1);
            }
        }

        /// <summary>
        /// Gets or sets the name of the column that contains the event starting date and time (must be convertible to DateTime).
        /// </summary>
        [Description("The name of the column that contains the event starting date and time (must be convertible to DateTime).")]
        [Category("Data")]
        public string DataStartField
        {
            get
            {
                return _dataStartField;
            }
            set
            {
                _dataStartField = value;

                if (Initialized)
                {
                    OnDataPropertyChanged();
                }

            }
        }

        /// <summary>
        /// Gets or sets the name of the column that contains the event ending date and time (must be convertible to DateTime).
        /// </summary>
        [Description("The name of the column that contains the event ending date and time (must be convertible to DateTime).")]
        [Category("Data")]
        public string DataEndField
        {
            get
            {
                return _dataEndField;
            }
            set
            {
                _dataEndField = value;
                if (Initialized)
                {
                    OnDataPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the  name of the column that contains the name of an event.
        /// </summary>
        [Category("Data")]
        [Description("The name of the column that contains the name of an event.")]
        public string DataTextField
        {
            get
            {
                return _dataTextField;
            }
            set
            {
                _dataTextField = value;

                if (Initialized)
                {
                    OnDataPropertyChanged();
                }

            }
        }

        /// <summary>
        /// Gets or sets the name of the column that contains the primary key. The primary key will be used for rendering the custom JavaScript actions.
        /// </summary>
        [Category("Data")]
        [Description("The name of the column that contains the primary key. The primary key will be used for rendering the custom JavaScript actions.")]
        [Obsolete("Use DataIdField instead.")]
        public string DataValueField
        {
            get { return DataIdField; }
            set { DataIdField = value; }
        }


        /// <summary>
        /// Gets or sets the name of the column that contains the primary key. The primary key will be used for rendering the custom JavaScript actions.
        /// </summary>
        [Category("Data")]
        [Description("The name of the column that contains the primary key. The primary key will be used for rendering the custom JavaScript actions.")]
        public string DataIdField
        {
            get
            {
                return _dataIdField;
            }
            set
            {
                _dataIdField = value;

                if (Initialized)
                {
                    OnDataPropertyChanged();
                }

            }
        }

        /// <summary>
        /// Gets or sets whether the hour numbers should be visible. Not supported since 5.0.
        /// </summary>
        [Category("Appearance")]
        [Description("Should the hour numbers be visible?")]
        [DefaultValue(true)]
        [Obsolete("Not supported since 5.0.")]
        public bool ShowHours
        {
            get
            {
                if (ViewState["ShowHours"] == null)
                    return true;
                return (bool)ViewState["ShowHours"];
            }
            set
            {
                ViewState["ShowHours"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the time-format for hour numbers (on the left).
        /// </summary>
        [Category("Appearance")]
        [Description("The time-format that will be used for the hour numbers.")]
        [DefaultValue(TimeFormat.Clock12Hours)]
        public TimeFormat TimeFormat
        {
            get
            {
                if (ViewState["TimeFormat"] == null)
                    return TimeFormat.Clock12Hours;
                return (TimeFormat)ViewState["TimeFormat"];
            }
            set
            {
                ViewState["TimeFormat"] = value;
            }
        }

        /// <summary>
        /// Handling of user action (clicking an event).
        /// </summary>
        [Category("User actions")]
        [Description("Whether clicking an event should do a postback or run a javascript action. By default, it calls the javascript code specified in EventClickJavaScript property.")]
        [DefaultValue(EventClickHandlingEnum.JavaScript)]
        public EventClickHandlingEnum EventClickHandling
        {
            get
            {
                if (ViewState["EventClickHandling"] == null)
                    return EventClickHandlingEnum.Disabled;
                return (EventClickHandlingEnum)ViewState["EventClickHandling"];
            }
            set
            {
                ViewState["EventClickHandling"] = value;
            }
        }

        /// <summary>
        /// Handling of user action (clicking a free-time slot).
        /// </summary>
        [Category("User actions")]
        [Description("Whether clicking a free-time slot should do a postback or run a javascript action. By default, it calls the javascript code specified in TimeRangeSelectedJavaScript property.")]
        [DefaultValue(TimeRangeSelectedHandling.Disabled)]
        public TimeRangeSelectedHandling TimeRangeSelectedHandling
        {
            get
            {
                if (ViewState["TimeRangeSelectedHandling"] == null)
                    return TimeRangeSelectedHandling.Disabled;
                return (TimeRangeSelectedHandling)ViewState["TimeRangeSelectedHandling"];
            }
            set
            {
                ViewState["TimeRangeSelectedHandling"] = value;
            }
        }

        //headerDateFormat
        /// <summary>
        /// Gets or sets the format of the date display in the header columns.
        /// </summary>
        [Description("Format of the date display in the header columns.")]
        [Category("Appearance")]
        [DefaultValue("d")]
        public string HeaderDateFormat
        {
            get
            {
                return _headerDateFormat;
            }
            set
            {
                _headerDateFormat = value;
            }
        }

        /// <summary>
        /// Gets or sets whether the header should be visible. Not supported since 5.0.
        /// </summary>
        [Category("Appearance")]
        [Description("Should the header be visible?")]
        [DefaultValue(true)]
        [Obsolete("Not supported since 5.0.")]
        public bool ShowHeader
        {
            get
            {
                return _showHeader;
            }
            set
            {
                _showHeader = value;
            }
        }


        /// <summary>
        /// Gets or sets whether the header should be visible.
        /// </summary>
        [Category("Layout")]
        [Description("Header height in pixels.")]
        [DefaultValue(21)]
        public int HeaderHeight
        {
            get
            {
                return _headerHeight;
            }
            set
            {
                _headerHeight = value;
            }
        }

        [Obsolete("This property is ignored since 5.0. Use a CSS theme instead.")]
        public override Color BackColor
        {
            get
            {
                if (ViewState["BackColor"] == null)
                    return ColorTranslator.FromHtml("#FFFFD5");
                return (Color)ViewState["BackColor"];
            }
            set
            {
                ViewState["BackColor"] = value;
            }
        }

        [Obsolete("This property is ignored since 5.0. Use a CSS theme instead.")]
        public override Color BorderColor
        {
            get
            {
                if (ViewState["BorderColor"] == null)
                    return ColorTranslator.FromHtml("#000000");
                return (Color)ViewState["BorderColor"];
            }
            set
            {
                ViewState["BorderColor"] = value;
            }
        }

        [Category("Appearance")]
        [TypeConverter(typeof(WebColorConverter))]
        public Color HoverColor
        {
            get
            {
                if (ViewState["HoverColor"] == null)
                    return ColorTranslator.FromHtml("#FFED95");
                return (Color)ViewState["HoverColor"];

            }
            set
            {
                ViewState["HoverColor"] = value;
            }
        }

        [Category("Appearance")]
        [TypeConverter(typeof(WebColorConverter))]
        [Obsolete("This property is ignored since 5.0. Use a CSS theme instead.")]
        public Color HourBorderColor
        {
            get
            {
                if (ViewState["HourBorderColor"] == null)
                    return ColorTranslator.FromHtml("#EAD098");
                return (Color)ViewState["HourBorderColor"];

            }
            set
            {
                ViewState["HourBorderColor"] = value;
            }
        }

        [Category("Appearance")]
        [TypeConverter(typeof(WebColorConverter))]
        [Obsolete("This property is ignored since 5.0. Use a CSS theme instead.")]
        public Color HourHalfBorderColor
        {
            get
            {
                if (ViewState["HourHalfBorderColor"] == null)
                    return ColorTranslator.FromHtml("#F3E4B1");
                return (Color)ViewState["HourHalfBorderColor"];

            }
            set
            {
                ViewState["HourHalfBorderColor"] = value;
            }
        }

        [Category("Appearance")]
        [TypeConverter(typeof(WebColorConverter))]
        [Obsolete("This property is ignored since 5.0. Use a CSS theme instead.")]
        public Color HourNameBorderColor
        {
            get
            {
                if (ViewState["HourNameBorderColor"] == null)
                    return ColorTranslator.FromHtml("#ACA899");
                return (Color)ViewState["HourNameBorderColor"];
            }
            set
            {
                ViewState["HourNameBorderColor"] = value;
            }
        }

        [Category("Appearance")]
        [TypeConverter(typeof(WebColorConverter))]
        [Obsolete("This property is ignored since 5.0. Use a CSS theme instead.")]
        public Color HourNameBackColor
        {
            get
            {
                if (ViewState["HourNameBackColor"] == null)
                    return ColorTranslator.FromHtml("#ECE9D8");
                return (Color)ViewState["HourNameBackColor"];
            }
            set
            {
                ViewState["HourNameBackColor"] = value;
            }
        }


        [Category("Appearance")]
        [TypeConverter(typeof(WebColorConverter))]
        [Obsolete("This property is ignored since 5.0. Use a CSS theme instead.")]
        public Color EventBackColor
        {
            get
            {
                if (ViewState["EventBackColor"] == null)
                    return ColorTranslator.FromHtml("#FFFFFF");
                return (Color)ViewState["EventBackColor"];
            }
            set
            {
                ViewState["EventBackColor"] = value;
            }
        }


        [Category("Appearance")]
        [TypeConverter(typeof(WebColorConverter))]
        [Obsolete("This property is ignored since 5.0. Use a CSS theme instead.")]
        public Color EventHoverColor
        {
            get
            {
                if (ViewState["EventHoverColor"] == null)
                    return ColorTranslator.FromHtml("#DCDCDC");
                return (Color)ViewState["EventHoverColor"];
            }
            set
            {
                ViewState["EventHoverColor"] = value;
            }
        }


        [Category("Appearance")]
        [TypeConverter(typeof(WebColorConverter))]
        [Obsolete("This property is ignored since 5.0. Use a CSS theme instead.")]
        public Color EventBorderColor
        {
            get
            {
                if (ViewState["EventBorderColor"] == null)
                    return ColorTranslator.FromHtml("#000000");
                return (Color)ViewState["EventBorderColor"];
            }
            set
            {
                ViewState["EventBorderColor"] = value;
            }
        }

        [Category("Appearance")]
        [TypeConverter(typeof(WebColorConverter))]
        [Obsolete("This property is ignored since 5.0. Use a CSS theme instead.")]
        public Color DurationBarColor
        {
            get
            {
                if (ViewState["DurationBarColor"] == null)
                    return ColorTranslator.FromHtml("blue");
                return (Color)ViewState["DurationBarColor"];
            }
            set
            {
                ViewState["DurationBarColor"] = value;
            }
        }

        [Category("Appearance")]
        [TypeConverter(typeof(WebColorConverter))]
        [Obsolete("This property is ignored since 5.0. Use a CSS theme instead.")]
        public Color NonBusinessBackColor
        {
            get
            {
                if (ViewState["NonBusinessBackColor"] == null)
                    return ColorTranslator.FromHtml("#FFF4BC");

                return (Color)ViewState["NonBusinessBackColor"];
            }
            set
            {
                ViewState["NonBusinessBackColor"] = value;
            }
        }

        [Category("Appearance")]
        [Obsolete("This property is ignored since 5.0. Use a CSS theme instead.")]
        public string EventFontFamily
        {
            get
            {
                if (ViewState["EventFontFamily"] == null)
                    return "Tahoma";

                return (string)ViewState["EventFontFamily"];
            }
            set
            {
                ViewState["EventFontFamily"] = value;
            }
        }


        [Category("Appearance")]
        [Obsolete("This property is ignored since 5.0. Use a CSS theme instead.")]
        public string HourFontFamily
        {
            get
            {
                if (ViewState["HourFontFamily"] == null)
                    return "Tahoma";

                return (string)ViewState["HourFontFamily"];
            }
            set
            {
                ViewState["HourFontFamily"] = value;
            }
        }

        [Category("Appearance")]
        [Obsolete("This property is ignored since 5.0. Use a CSS theme instead.")]
        public string DayFontFamily
        {
            get
            {
                if (ViewState["DayFontFamily"] == null)
                    return "Tahoma";

                return (string)ViewState["DayFontFamily"];
            }
            set
            {
                ViewState["DayFontFamily"] = value;
            }
        }

        [Category("Appearance")]
        [Obsolete("This property is ignored since 5.0. Use a CSS theme instead.")]
        public string EventFontSize
        {
            get
            {
                if (ViewState["EventFontSize"] == null)
                    return "8pt";

                return (string)ViewState["EventFontSize"];
            }
            set
            {
                ViewState["EventFontSize"] = value;
            }
        }

        [Category("Appearance")]
        [Obsolete("This property is ignored since 5.0. Use a CSS theme instead.")]
        public string HourFontSize
        {
            get
            {
                if (ViewState["HourFontSize"] == null)
                    return "16pt";

                return (string)ViewState["HourFontSize"];
            }
            set
            {
                ViewState["HourFontSize"] = value;
            }
        }

        [Category("Appearance")]
        [Obsolete("This property is ignored since 5.0. Use a CSS theme instead.")]
        public string DayFontSize
        {
            get
            {
                if (ViewState["DayFontSize"] == null)
                    return "10pt";

                return (string)ViewState["DayFontSize"];
            }
            set
            {
                ViewState["DayFontSize"] = value;
            }
        }

        /// <summary>
        /// Determines whether the event tooltip is active.
        /// </summary>
        [Description("Determines whether the event tooltip is active.")]
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool ShowToolTip
        {
            get
            {
                if (ViewState["ShowToolTip"] == null)
                    return true;
                return (bool)ViewState["ShowToolTip"];
            }
            set
            {
                ViewState["ShowToolTip"] = value;
            }
        }

        /// <summary>
        /// Width of the right margin inside a column (in pixels).
        /// </summary>
        [Description("Width of the right margin inside a column (in pixels).")]
        [Category("Appearance")]
        [DefaultValue(5)]
        public int ColumnMarginRight
        {
            get
            {
                if (ViewState["ColumnMarginRight"] == null)
                    return 5;
                return (int)ViewState["ColumnMarginRight"];
            }
            set
            {
                ViewState["ColumnMarginRight"] = value;
            }
        }

        /// <summary>
        /// Hide non-business cells if there are no events. Works for HeightSpec="Full". Not supported since 5.0.
        /// </summary>
        [Category("Behavior")]
        [Description("Hide non-business cells if there are no events.")]
        [DefaultValue(false)]
        [Obsolete("Not supported since 5.0.")]
        public bool HideFreeCells
        {
            get
            {
                if (ViewState["HideFreeCells"] == null)
                    return false;
                return (bool)ViewState["HideFreeCells"];
            }
            set
            {
                ViewState["HideFreeCells"] = value;
            }
        }

        /// <summary>
        /// Sets or get the way how the height of the scrolling area is determined. It can be either Full (the full height, prevents scrolling), or BusinessHoursNoScroll (it always shows business hours in full).
        /// </summary>
        [DefaultValue(HeightSpecEnum.BusinessHours)]
        [Category("Layout")]
        [Description("Sets or get the way how the height of the scrolling area is determined - Full (the full height, prevents scrolling), or BusinessHoursNoScroll (it always shows business hours in full).")]
        public HeightSpecEnum HeightSpec
        {
            get
            {
                if (ViewState["HeightSpec"] == null)
                    return HeightSpecEnum.BusinessHours;

                return (HeightSpecEnum)ViewState["HeightSpec"];
            }
            set
            {
                ViewState["HeightSpec"] = value;
            }
        }

        [Category("Appearance")]
        [DefaultValue(true)]
        [Obsolete("This property is ignored since 5.0. CssOnly mode is always enabled.")]
        public bool CssOnly
        {
            get
            {
                return true;
            }
            set { ; }
        }

        /// <summary>
        /// Specifies the prefix of the CSS classes that contain style definitions for the elements of this control.
        /// </summary>
        [Category("Appearance")]
        [Description("Specifies the prefix of the CSS classes that contain style definitions for the elements of this control. Obsolete: Use Theme property instead.")]
        [Obsolete("Use Theme property instead.")]
        public string CssClassPrefix
        {
            get
            {
                return Theme;
            }
            set
            {
                Theme = value;
            }
        }

        /// <summary>
        /// Specifies the CSS theme.
        /// </summary>
        [Category("Appearance")]
        [Description("Specifies the CSS theme.")]
        [DefaultValue("calendar_default")]
        public string Theme
        {
            get
            {
                if (ViewState["Theme"] == null)
                {
                    return "calendar_default";
                }
                return (string)ViewState["Theme"];
            }
            set
            {
                ViewState["Theme"] = value;
            }
        }

        /// <summary>
        /// JavaScript instance name on the client-side. If it is not specified the control ClientID will be used.
        /// </summary>
        [Description("JavaScript instance name on the client-side. If it is not specified the control ClientID will be used.")]
        public string ClientObjectName
        {
            get
            {
                if (String.IsNullOrEmpty(ViewState["ClientObjectName"] as string))
                {
                    if (DesignMode)
                        return null;
                    return ClientID;
                }
                return (string)ViewState["ClientObjectName"];
            }
            set
            {
                ViewState["ClientObjectName"] = value;
            }
        }

        /// <summary>
        /// Whether the color bar on the left side of and event should be visible.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("Whether the color bar on the left side of and event should be visible.")]
        public bool DurationBarVisible
        {
            get
            {
                if (ViewState["DurationBarVisible"] == null)
                    return true;
                return (bool)ViewState["DurationBarVisible"];
            }
            set
            {
                ViewState["DurationBarVisible"] = value;
            }
        }

        /// <summary>
        /// Sets or gets the view type (day, week, custom number of days, custom column set). If set to <see cref="ViewTypeEnum.Resources">ViewTypeEnum.Resources</see> and you use data binding you have to specify <see cref="DataColumnField">DataColumnField</see> property.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(ViewTypeEnum.Days)]
        [Description("Sets the view type. In days view it shows one or more days in the columns. In in resources view it shows multiple resources in the columns.")]
        public ViewTypeEnum ViewType
        {
            get
            {
                if (ViewState["ViewType"] == null)
                    return ViewTypeEnum.Days;

                return (ViewTypeEnum)ViewState["ViewType"];
            }
            set
            {
                ViewState["ViewType"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of days to be displayed. Default is 1.
        /// </summary>
        [Description("The number of days to be displayed on the calendar. Default value is 1.")]
        [Category("Behavior")]
        [DefaultValue(1)]
        public int Days
        {
            get
            {
                switch (ViewType)
                {
                    case ViewTypeEnum.Day:
                        return 1;
                    case ViewTypeEnum.Week:
                        return 7;
                    case ViewTypeEnum.WorkWeek:
                        return 5;
                }


                if (ViewState["Days"] == null)
                    return 1;
                return (int)ViewState["Days"];
            }
            set
            {
                int daysCount = value;

                if (daysCount < 1)
                    daysCount = 1;

                ViewState["Days"] = daysCount;
            }
        }

        /// <summary>
        /// Sets or get the starting scroll position of the scrolling area (in hours). Does not apply when <see cref="HeightSpec">HeightSpec</see> is set to <see cref="HeightSpecEnum.Full">HeightSpecEnum.Full</see> or <see cref="HeightSpecEnum.BusinessHoursNoScroll">HeightSpecEnum.BusinessHoursNoScroll</see>.
        /// </summary>
        [Category("Behavior")]
        [Description("Sets the starting scroll position of the scrolling area (in hours). Does not apply when HeightSpec property is set to HeightSpecEnum.Full.")]
        public int ScrollPositionHour
        {
            get
            {
                if (ViewState["ScrollPositionHour"] == null)
                    return BusinessBeginsHour;

                return (int)ViewState["ScrollPositionHour"];
            }
            set
            {
                if (value != BusinessBeginsHour)
                    ViewState["ScrollPositionHour"] = value;
            }
        }
        #endregion

        public void RaiseCallbackEvent(string ea)
        {
            _callbackException = null;
            //CallBackAction = CallBackAction.Events;

            try
            {
                string action = ea.Substring(0, 4);
                if (action == "JSON")
                {
                    ExecuteEventJson(ea.Substring(4));
                }
                else
                {
                    throw new Exception("Unsupported CallBack data format.");
                }
            }
            catch (Exception e)
            {
                _callbackException = e;
                throw;
            }
        }

        internal string ResolvedHeaderDateFormat
        {
            get
            {

                switch (HeaderDateFormat)
                {
                    case "d":
                        return Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
                    case "D":
                        return Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern;
                    case "M":
                        return Thread.CurrentThread.CurrentCulture.DateTimeFormat.MonthDayPattern;
                    case "m":
                        return Thread.CurrentThread.CurrentCulture.DateTimeFormat.MonthDayPattern;
                    case "%d":
                        return "d";
                    default:
                        return HeaderDateFormat;
                }
            }
        }


        private void ExecuteEventJson(string ea)
        {
            JsonData envelope = SimpleJsonDeserializer.Deserialize(ea);
            JsonData header = envelope["header"];
            JsonData data = envelope["data"];
            JsonData parameters = envelope["parameters"];

            // load header
            StartDate = (DateTime)header["startDate"];
            Days = (int)header["days"];
            ViewType = ViewTypeParser.Parse((string)header["viewType"]);

            BusinessBeginsHour = (int)header["businessBeginsHour"];
            BusinessEndsHour = (int)header["businessEndsHour"];

            _hashes = new Hashtable();
            _hashes["callBack"] = CallBack.GetHash();
            _hashes["columns"] = (string)header["hashes"]["columns"];
            _hashes["events"] = (string)header["hashes"]["events"];

            switch ((string)envelope["action"])
            {
                case "EventClick":
                    if (EventClick != null)
                    {
                        EventClickEventArgs e = new EventClickEventArgs(parameters, data);
                        e.Source = Page.IsCallback ? EventSource.CallBack : EventSource.PostBack;
                        EventClick(this, e);
                    }
                    break;

                case "EventMove":
                    if (EventMove != null)
                    {
                        EventMoveEventArgs e = new EventMoveEventArgs(parameters, data);
                        e.Source = Page.IsCallback ? EventSource.CallBack : EventSource.PostBack;
                        EventMove(this, e);
                    }
                    break;

                case "EventResize":
                    if (EventResize != null)
                    {
                        EventResizeEventArgs e = new EventResizeEventArgs(parameters, data);
                        e.Source = Page.IsCallback ? EventSource.CallBack : EventSource.PostBack;
                        EventResize(this, e);
                    }
                    break;

                case "Command":
                    if (Command != null)
                    {
                        CommandEventArgs e = new CommandEventArgs(parameters, data);
                        e.Source = Page.IsCallback ? EventSource.CallBack : EventSource.PostBack;
                        Command(this, e);
                    }
                    break;

                case "TimeRangeSelected":
                    if (TimeRangeSelected != null)
                    {
                        TimeRangeSelectedEventArgs e = new TimeRangeSelectedEventArgs(parameters, data);
                        e.Source = Page.IsCallback ? EventSource.CallBack : EventSource.PostBack;
                        TimeRangeSelected(this, e);
                    }
                    break;

                default:
                    throw new NotSupportedException("This action type is not supported: " + envelope["action"]);

            }

        }



        public string GetCallbackResult()
        {
            if (_callbackException != null)
            {
                return HttpContext.Current.IsDebuggingEnabled
                           ? "$$$" + _callbackException
                           : "$$$" + _callbackException.Message;
            }

            try
            {
                Hashtable result = new Hashtable();

                if (_callbackUpdateType == CallBackUpdateType.None)
                {
                    result["UpdateType"] = _callbackUpdateType.ToString();
                    return SimpleJsonSerializer.Serialize(result);
                }

                List<Hashtable> events = GetEvents();

                result["Events"] = events;

                if (_callbackUpdateType == CallBackUpdateType.Auto || _callbackUpdateType == CallBackUpdateType.Full)
                {
                    Hashtable full = new Hashtable();

                    // properties
                    full["Days"] = Days;
                    full["StartDate"] = StartDate.ToString("s");
                    full["ViewType"] = ViewType;

                    full["BusinessBeginsHour"] = BusinessBeginsHour;
                    full["BusinessEndsHour"] = BusinessEndsHour;
                    full["HeaderDateFormat"] = ResolvedHeaderDateFormat;

                    Hashtable hashes = new Hashtable();
                    hashes["callBack"] = CallBack.GetHash();
                    hashes["events"] = Hash(events);

                    full["Hashes"] = hashes;

                    if (_callbackUpdateType == CallBackUpdateType.Auto)
                    {
                        _callbackUpdateType = DifferentHashes(hashes) ? CallBackUpdateType.Full : CallBackUpdateType.EventsOnly;
                        if (_callbackUpdateType == CallBackUpdateType.EventsOnly) // detected change of events only
                        {
                            bool noEventsChange = false;
                            if (!_databindCalled)
                            {
                                noEventsChange = true;
                            }
                            if (!DifferentHash(hashes, "events"))
                            {
                                noEventsChange = true;
                            }

                            if (noEventsChange)
                            {
                                _callbackUpdateType = CallBackUpdateType.None;
                                result.Remove("Events"); // don't send it to save bandwidth
                            }

                        }

                    }

                    if (_callbackUpdateType == CallBackUpdateType.Full)
                    {
                        foreach (string key in full.Keys)
                        {
                            result[key] = full[key];
                        }
                    }
                }

                if (_callbackUpdateType == CallBackUpdateType.EventsOnly)
                {
                    Hashtable hashes = new Hashtable();
                    hashes["events"] = Hash(events);
                    result["Hashes"] = hashes;
                }

                if (_callbackUpdateType != CallBackUpdateType.None)
                {
                    if (EnableViewState)
                    {
                        // ViewState sync
                        using (StringWriter vs = new StringWriter())
                        {
                            LosFormatter f = new LosFormatter();
                            f.Serialize(vs, ViewStateHelper.ToHashtable(ViewState));
                            result["VsUpdate"] = vs.ToString();
                        }
                    }

                }

                result["UpdateType"] = _callbackUpdateType.ToString();

                return SimpleJsonSerializer.Serialize(result);
            }
            catch (Exception e)
            {
                return HttpContext.Current.IsDebuggingEnabled ? "$$$" + e : "$$$" + e.Message;
            }

        }

        /// <summary>
        /// A shortcut for all properties that can be modified during a CallBack update.
        /// </summary>
        public DayPilotCalendarCallBack CallBack
        {
            get { return _callback ?? (_callback = new DayPilotCalendarCallBack(this)); }
        }

        internal string Hash(object data)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(SimpleJsonSerializer.Serialize(data));
            return Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(bytes));
        }


        /// <summary>
        /// Compares all keys but "events".
        /// </summary>
        /// <param name="hashes"></param>
        /// <returns></returns>
        private bool DifferentHashes(Hashtable hashes)
        {
            bool different = false;
            foreach (string key in hashes.Keys)
            {
                if (key == "events") // different events don't invoke full update
                {
                    continue;
                }
                if (DifferentHash(hashes, key))
                {
                    different = true;
                }
            }
            return different;
        }

        private bool DifferentHash(Hashtable hashes, string key)
        {
            return (string)hashes[key] != (string)_hashes[key];
        }


        /// <summary>
        /// Determines the action to be executed after a user moves an event. If set to Disabled moving is not enabled  on the client side.
        /// </summary>
        [Category("Events")]
        [Description("Determines the action to be executed after a user moves an event. If set to Disabled moving is not enabled  on the client side.")]
        [DefaultValue(UserActionHandling.Disabled)]
        public UserActionHandling EventMoveHandling
        {
            get
            {
                if (ViewState["EventMoveHandling"] == null)
                {
                    return UserActionHandling.Disabled;
                }

                return (UserActionHandling)ViewState["EventMoveHandling"];

            }
            set
            {
                ViewState["EventMoveHandling"] = value;
            }
        }

        /// <summary>
        /// Determines the action to be executed after a user resizes an event. If set to Disabled resizing is not enabled on the client side.
        /// </summary>
        [Category("Events")]
        [Description("Determines the action to be executed after a user resizes an event. If set to Disabled resizing is not enabled on the client side.")]
        [DefaultValue(UserActionHandling.Disabled)]
        public UserActionHandling EventResizeHandling
        {
            get
            {
                if (ViewState["EventResizeHandling"] == null)
                {
                    return UserActionHandling.Disabled;
                }

                return (UserActionHandling)ViewState["EventResizeHandling"];

            }
            set
            {
                ViewState["EventResizeHandling"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Javascript code that is executed when a user moves an event.
        /// </summary>
        [Description("Javascript function that is executed when a users moves an event.")]
        [Category("Events")]
        [DefaultValue("alert('Event with id ' + e.id() + ' was moved.');")]
        public string EventMoveJavaScript
        {
            get
            {
                if (ViewState["EventMoveJavaScript"] == null)
                    return "alert('Event with id ' + e.id() + ' was moved.');";
                return (string)ViewState["EventMoveJavaScript"];
            }
            set
            {
                ViewState["EventMoveJavaScript"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Javascript function that is executed when a user moves an event.
        /// </summary>
        [Description("Javascript function that is executed when a users moves an event.")]
        [Category("Events")]
        [DefaultValue("alert('Event with id ' + e.id() + ' was resized.');")]
        public string EventResizeJavaScript
        {
            get
            {
                if (ViewState["EventResizeJavaScript"] == null)
                    return "alert('Event with id ' + e.id() + ' was resized.');";
                return (string)ViewState["EventResizeJavaScript"];
            }
            set
            {
                ViewState["EventResizeJavaScript"] = value;
            }
        }

        /// <summary>
        /// Request for redraw of the Calendar on the client side. Detects the changes and automatically uses EventsOnly or Full update mode as needed.
        /// </summary>
        public void Update()
        {
            _callbackUpdateType = CallBackUpdateType.Auto;
        }

        /// <summary>
        /// Whether to show the event start and end times.
        /// </summary>
        [Description("Whether to show the event start and end times.")]
        [Category("Appearance")]
        [DefaultValue(false)]
        public bool ShowEventStartEnd
        {
            get
            {
                if (ViewState["ShowEventStartEnd"] == null)
                    return false;
                return (bool)ViewState["ShowEventStartEnd"];
            }
            set
            {
                ViewState["ShowEventStartEnd"] = value;
            }
        }

        public DateTime VisibleStart
        {
            get { return StartDate; }
        }

        public DateTime VisibleEnd
        {
            get { return StartDate.AddDays(Days); }
        }

    }
}
