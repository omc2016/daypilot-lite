<%@ Page Language="C#" MasterPageFile="~/Demo.master" AutoEventWireup="true" CodeFile="UpdatePanel.aspx.cs"
    Inherits="UpdatePanel" Title="AJAX Calendar (UpdatePanel) | DayPilot Lite for ASP.NET WebForms Demo" %>

<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
<div style="color:red; font-weight: bold;">UpdatePanel1:</div>
    <div style="border:1px solid red; padding: 10px;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Calendar ID="Calendar1" runat="server" CssClass="calendar" OnSelectionChanged="Calendar1_SelectionChanged" DayNameFormat="FirstTwoLetters">
                <TodayDayStyle BorderColor="Red" BorderStyle="Solid" BorderWidth="1px"></TodayDayStyle>
                <SelectedDayStyle BackColor="#FBE694" ForeColor="Black" CssClass="selected"></SelectedDayStyle>
                <TitleStyle BackColor="White"></TitleStyle>
                <OtherMonthDayStyle ForeColor="#ACA899"></OtherMonthDayStyle>
            </asp:Calendar>
            <br />
            <DayPilot:DayPilotCalendar 
                ID="DayPilotCalendar1" 
                runat="server" 
                DataEndField="end"
                DataStartField="start" 
                DataTextField="name" 
                DataValueField="id" 
                ViewType="Week" 
                NonBusinessHours="Hide" 

                OnEventClick="DayPilotCalendar1_EventClick"

                TimeRangeSelectedHandling="PostBack"
                OnTimeRangeSelected="DayPilotCalendar1_TimeRangeSelected" 
                
                EventMoveHandling="PostBack"
                OnEventMove="DayPilotCalendar1_OnEventMove"
                
                EventResizeHandling="PostBack"
                OnEventResize="DayPilotCalendar1_OnEventResize"

                ></DayPilot:DayPilotCalendar>
        </ContentTemplate>
    </asp:UpdatePanel>
        </div>    
</asp:Content>
