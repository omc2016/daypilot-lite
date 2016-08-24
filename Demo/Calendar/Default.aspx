<%@ Page Language="C#" MasterPageFile="~/Demo.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" 
Inherits="_Default" Title="Calendar Control | DayPilot Lite for ASP.NET WebForms Demo" Culture="en-US" %>
<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
<div class="note"><b>Note:</b> Read more about the <a href="http://doc.daypilot.org/calendar/">ASP.NET event calendar</a>.</div>

<DayPilot:DayPilotCalendar ID="DayPilotCalendar1" runat="server" 
    
    DataTextField="name" 
    DataValueField="id" 
    StartDate="2007-01-01" 
    TimeFormat="Clock12Hours" 
    DataStartField="Start" 
    DataEndField="End" 
    Days="7" 
    NonBusinessHours="Hide" 
    onbeforeeventrender="DayPilotCalendar1_BeforeEventRender"
        
    TimeRangeSelectedHandling="CallBack"
    OnTimeRangeSelected="DayPilotCalendar1_OnTimeRangeSelected"
    
    EventClickHandling="PostBack"
    EventClickJavaScript="alert(e.id());"
    OnEventClick="DayPilotCalendar1_OnEventClick"
    
    EventMoveHandling="CallBack"
    OnEventMove="DayPilotCalendar1_OnEventMove"
    
    EventResizeHandling="CallBack"
    OnEventResize="DayPilotCalendar1_OnEventResize"
        
        >
        </DayPilot:DayPilotCalendar>
</asp:Content>

