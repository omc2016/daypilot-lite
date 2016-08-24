<%@ Page Language="C#" MasterPageFile="~/Demo.master" AutoEventWireup="true" CodeFile="HourHeight.aspx.cs" 
Inherits="HourHeight" Title="Hour Cell Height | DayPilot Lite for ASP.NET WebForms Demo" Culture="en-US" %>

<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="note"><b>Note:</b> Read more about the event calendar <a href="http://doc.daypilot.org/calendar/cell-height/">cell height</a>.</div>
    Cell height:
    <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
        <asp:ListItem Value="15" Selected="True">15</asp:ListItem>
        <asp:ListItem Value="17">17</asp:ListItem>
        <asp:ListItem>20</asp:ListItem>
        <asp:ListItem>25</asp:ListItem>
    </asp:DropDownList><br />
    <br />
    <daypilot:daypilotcalendar id="DayPilotCalendar1" runat="server" DataEndField="end" DataStartField="start" DataTextField="name" DataValueField="id" Width="100%" HourHeight="30"
    ></daypilot:daypilotcalendar>
</asp:Content>

