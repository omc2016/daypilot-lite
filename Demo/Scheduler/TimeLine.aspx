<%@ Page Language="C#" MasterPageFile="~/Demo.master" AutoEventWireup="true" CodeFile="TimeLine.aspx.cs" 
Inherits="SchedulerTimeline" Title="Timeline | DayPilot Lite for ASP.NET WebForms Demo" %>
<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:Label ID="Label1" runat="server" Text="Label" Font-Bold="true" Font-Size="16pt"></asp:Label><br />
<DayPilot:DayPilotScheduler ID="DayPilotScheduler1" runat="server" 
        HeaderFontSize="8pt" HeaderHeight="20" 
        DataStartField="start" 
        DataEndField="end" 
        DataTextField="empty" 
        DataValueField="id" 
        DataResourceField="resource" 
        EventFontSize="11px" 
        CellDuration="1440" 
        DurationBarVisible="false"
        OnBeforeEventRender="DayPilotScheduler1_OnBeforeEventRender"

        EventHeight="25"

        >
        <Resources>
            <DayPilot:Resource Name="Preparation" Value="A" />
            <DayPilot:Resource Name="Phase 1" Value="B" />
            <DayPilot:Resource Name="Phase 2" Value="C" />
            <DayPilot:Resource Name="Phase 3" Value="D" />
            <DayPilot:Resource Name="Validation" Value="E" />
            <DayPilot:Resource Name="Feedback" Value="F" />
        </Resources>
    </DayPilot:DayPilotScheduler>

</asp:Content>

