<%@ Page Language="C#" MasterPageFile="~/Demo.master" AutoEventWireup="true" CodeFile="ThemeTraditional.aspx.cs" 
Inherits="ThemeTraditional" Title="Traditional CSS Theme (Scheduler) | DayPilot Lite for ASP.NET WebForms Demo" %>
<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:Label ID="Label1" runat="server" Text="Label" Font-Bold="true" Font-Size="16pt"></asp:Label><br />

<div class="note"><b>Note:</b> You can create a theme using the online <strong>DayPilot Theme Designer</strong>: <a href="http://themes.daypilot.org/">http://themes.daypilot.org/</a></div>

<DayPilot:DayPilotScheduler ID="DayPilotScheduler1" runat="server" 
        HeaderFontSize="8pt" HeaderHeight="20" 
        DataStartField="start" 
        DataEndField="end" 
        DataTextField="name" 
        DataValueField="id" 
        DataResourceField="resource" 
        EventFontSize="11px" 
        CellDuration="1440" 
        OnBeforeEventRender="DayPilotScheduler1_BeforeEventRender"
        EventClickHandling="JavaScript"
        TimeRangeSelectedHandling="JavaScript"

        CssOnly="true"
        CssClassPrefix="scheduler_traditional"
        EventHeight="30"
        >
        <Resources>
            <DayPilot:Resource Name="Room A" Value="A" />
            <DayPilot:Resource Name="Room B" Value="B" />
            <DayPilot:Resource Name="Room C" Value="C" />
            <DayPilot:Resource Name="Room D" Value="D" />
            <DayPilot:Resource Name="Room E" Value="E" />
            <DayPilot:Resource Name="Room F" Value="F" />
            <DayPilot:Resource Name="Room G" Value="G" />
            <DayPilot:Resource Name="Room H" Value="H" />
            <DayPilot:Resource Name="Room I" Value="I" />
            <DayPilot:Resource Name="Room J" Value="J" />
            <DayPilot:Resource Name="Room K" Value="K" />
        </Resources>
    </DayPilot:DayPilotScheduler>

</asp:Content>

