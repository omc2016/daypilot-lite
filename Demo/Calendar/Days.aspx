<%@ Page Language="C#" MasterPageFile="~/Demo.master" AutoEventWireup="true" CodeFile="Days.aspx.cs" 
Inherits="Days" Title=" Calendar (Days View) | DayPilot Lite for ASP.NET WebForms Demo" Culture="en-US" %>

<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <div class="note"><b>Note:</b> Read more about the event calendar <a href="http://doc.daypilot.org/calendar/days-view/">days view</a>.</div>


    How many days to show:
    <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
        <asp:ListItem Value="1">1</asp:ListItem>
        <asp:ListItem Value="2">2</asp:ListItem>
        <asp:ListItem>3</asp:ListItem>
        <asp:ListItem Selected="True">4</asp:ListItem>
        <asp:ListItem>5</asp:ListItem>
        <asp:ListItem>6</asp:ListItem>
        <asp:ListItem>7</asp:ListItem>
        <asp:ListItem>8</asp:ListItem>
    </asp:DropDownList><br />
    <br />
    <daypilot:daypilotcalendar id="DayPilotCalendar1" runat="server" DataEndField="end" DataStartField="start" DataTextField="name" DataValueField="id" Days="4"
    ></daypilot:daypilotcalendar>
</asp:Content>

