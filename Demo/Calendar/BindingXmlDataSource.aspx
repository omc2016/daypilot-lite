<%@ Page Language="C#" MasterPageFile="~/Demo.master" AutoEventWireup="true" CodeFile="BindingXmlDataSource.aspx.cs" 
Inherits="BindingXmlDataSource" Title="Binding Calendar to XmlDataSource | DayPilot Lite for ASP.NET WebForms Demo" Culture="en-US" %>
<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<DayPilot:DayPilotCalendar ID="DayPilotCalendar1" runat="server" DataSourceID="XmlDataSource1" DataTextField="name" DataValueField="id" StartDate="2014-06-01" TimeFormat="Clock12Hours" DataStartField="Start" DataEndField="End"
 >
        </DayPilot:DayPilotCalendar>
    &nbsp;<br />
    <asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/App_Data/TestingData.xml">
    </asp:XmlDataSource>
</asp:Content>

