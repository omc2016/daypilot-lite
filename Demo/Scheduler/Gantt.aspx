<%@ Page Language="C#" MasterPageFile="~/Demo.master" AutoEventWireup="true" CodeFile="Gantt.aspx.cs" 
Inherits="Gantt" Title="Gantt | DayPilot Lite for ASP.NET WebForms Demo" %>
<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
<div class="note"><b>Note:</b> Read more about the scheduler <a href="http://doc.daypilot.org/scheduler/gantt-chart/">Gantt chart</a> mode.</div>    

<asp:Label ID="Label1" runat="server" Text="Label" Font-Bold="true" Font-Size="16pt"></asp:Label><br />

<DayPilot:DayPilotScheduler ID="DayPilotScheduler1" runat="server" 
        HeaderFontSize="8pt" 
        HeaderHeight="20" 
        DataStartField="start" 
        DataEndField="end" 
        DataTextField="name" 
        DataValueField="id" 
        DataResourceField="resource" 
        EventFontSize="11px" 
        CellDuration="1440" 
        CellWidth="20"
        Days="30" 
        ViewType="Gantt"

        EventHeight="25"
        >
    </DayPilot:DayPilotScheduler>

</asp:Content>

