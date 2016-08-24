<%@ Page Language="C#" MasterPageFile="~/Demo.master" AutoEventWireup="true" CodeFile="TimeFormat.aspx.cs" 
Inherits="TimeFormat" Title="Time Format (12/24 Hours) | DayPilot Lite for ASP.NET WebForms Demo" Culture="en-US" %>

<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <div class="note"><b>Note:</b> Read more about the event calendar <a href="http://doc.daypilot.org/calendar/time-format/">time format</a>.</div>

    <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
        <asp:ListItem Value="12">12-hours clock</asp:ListItem>
        <asp:ListItem Value="24" Selected="true">24-hours clock</asp:ListItem>
    </asp:DropDownList><br />
    <br />
    <daypilot:daypilotcalendar id="DayPilotCalendar1" runat="server" DataEndField="end" DataStartField="start" DataTextField="name" DataValueField="id" TimeFormat="Clock24Hours"
    ></daypilot:daypilotcalendar>
</asp:Content>

