<%@ Page Language="C#" MasterPageFile="~/Demo.master" AutoEventWireup="true" CodeFile="NonBusinessHours.aspx.cs" 
Inherits="NonBusinessHours" Title="Non-Business Hours | DayPilot Lite for ASP.NET WebForms Demo" Culture="en-US" %>
<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <div class="note"><b>Note:</b> Read more about controlling the event calendar <a href="http://doc.daypilot.org/calendar/height/">height</a>.</div>

    <div>
        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
            <asp:ListItem Value="scroll">Show busines hours, with scrollbar</asp:ListItem>
            <asp:ListItem Value="hide">Show business hours, without scrollbar</asp:ListItem>
            <asp:ListItem Value="show">Show full hours</asp:ListItem>
        </asp:DropDownList><br />
        <br />
        <daypilot:daypilotcalendar id="DayPilotCalendar1" runat="server" DataStartField="Start"
            dataendfield="End" datatextfield="Name" datavaluefield="Id" nonbusinesshours="Hide" Days="2"
            ></daypilot:daypilotcalendar>
    </div>
</asp:Content>