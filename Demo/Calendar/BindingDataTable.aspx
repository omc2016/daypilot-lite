<%@ Page Language="C#" MasterPageFile="~/Demo.master" AutoEventWireup="true" CodeFile="BindingDataTable.aspx.cs" 
Inherits="BindingDataTable" Title="Binding Calendar to DataTable | DayPilot Lite for ASP.NET WebForms Demo" Culture="en-US" %>

<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <daypilot:daypilotcalendar id="DayPilotCalendar1" runat="server" dataendfield="end"
        datastartfield="start" datatextfield="name" datavaluefield="id" HourHeight="34" EventHoverColor="Gainsboro" NonBusinessHours="Hide"
        
         ></daypilot:daypilotcalendar>
</asp:Content>

