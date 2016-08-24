<%@ Page Language="C#" MasterPageFile="~/Demo.master" AutoEventWireup="true" CodeFile="GanttColumns.aspx.cs" 
Inherits="GanttColumns" Title="Gantt with Columns | DayPilot Lite for ASP.NET WebForms Demo" %>
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
        EventFontSize="11px" 
        CellDuration="60" 
        CellWidth="40"
        Days="1" 
        ViewType="Gantt"
        OnBeforeResHeaderRender="DayPilotScheduler1_BeforeResHeaderRender"
        OnHeaderColumnWidthChanged="DayPilotScheduler1_RowHeaderColumnWidthChanged"

        EventHeight="25"
        >
        <HeaderColumns>
        <DayPilot:RowHeaderColumn Title="Event" Width="200" />
        <DayPilot:RowHeaderColumn Title="Duration" Width="100" />
        </HeaderColumns>
    </DayPilot:DayPilotScheduler>


<div>
Columns headers can be resized using drag&amp;drop.
</div>

</asp:Content>

