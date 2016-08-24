<%@ Page Language="C#" MasterPageFile="~/Demo.master" AutoEventWireup="true" CodeFile="ThemeWhite.aspx.cs" 
Inherits="ThemeWhite" Title="White CSS Theme - Calendar Control | DayPilot Lite for ASP.NET WebForms Demo" Culture="en-US" %>
<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="note"><b>Note:</b> You can create a theme using the online <strong>DayPilot Theme Designer</strong>: <a href="http://themes.daypilot.org/">http://themes.daypilot.org/</a></div>

<DayPilot:DayPilotCalendar ID="DayPilotCalendar1" runat="server" 
        DataTextField="name" 
        DataValueField="id" 
        TimeFormat="Clock12Hours" 
        DataStartField="Start" 
        DataEndField="End" 
        Days="7" 
        NonBusinessHours="Hide" 
        onbeforeeventrender="DayPilotCalendar1_BeforeEventRender"
        EventClickHandling="JavaScript"
        TimeRangeSelectedHandling="JavaScript"
        
        CssOnly="true"
        CssClassPrefix="calendar_white"
        >
        </DayPilot:DayPilotCalendar>
</asp:Content>

