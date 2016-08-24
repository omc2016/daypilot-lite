<%@ Page Language="C#" MasterPageFile="~/Demo.master" AutoEventWireup="true" CodeFile="WeekView.aspx.cs" 
Inherits="WeekView" Title="Calendar (Week) | DayPilot Lite for ASP.NET WebForms Demo" Culture="en-US" %>

<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="note"><b>Note:</b> Read more about the event calendar <a href="http://doc.daypilot.org/calendar/week-view/">week view</a>.</div>

    <daypilot:daypilotcalendar id="DayPilotCalendar1" runat="server" DataEndField="end" DataStartField="start" DataTextField="name" DataValueField="id" Days="7" StartDate="2014/06/01" DataSourceID="XmlDataSource1"
    ></daypilot:daypilotcalendar>
        <asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/App_Data/TestingData.xml">
    </asp:XmlDataSource>
</asp:Content>

