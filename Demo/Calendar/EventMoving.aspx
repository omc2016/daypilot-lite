<%@ Page Language="C#" MasterPageFile="~/Demo.master" AutoEventWireup="true" CodeFile="EventMoving.aspx.cs" 
Inherits="EventMoving" Title="Event Moving - Calendar Control | DayPilot Lite for ASP.NET WebForms Demo" Culture="en-US" %>
<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
<div class="note"><b>Note:</b> Read more about <a href="http://doc.daypilot.org/calendar/event-moving/">drag and drop event moving</a>.</div>

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
        
    EventMoveHandling="CallBack"
    OnEventMove="DayPilotCalendar1_OnEventMove"
        >
        </DayPilot:DayPilotCalendar>
</asp:Content>

