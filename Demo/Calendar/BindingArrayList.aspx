<%@ Page Language="C#" MasterPageFile="~/Demo.master" AutoEventWireup="true" CodeFile="BindingArrayList.aspx.cs" 
Inherits="BindingArrayList" Title="Binding Calendar to ArrayList | DayPilot Lite for ASP.NET WebForms Demo" Culture="en-US" %>

<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <daypilot:daypilotcalendar id="DayPilotCalendar1" runat="server" DataStartField="Start"
            dataendfield="End" datatextfield="Name" datavaluefield="Id"
            
            ></daypilot:daypilotcalendar>
    <br />
    <h2>
        ArrayListBinding.aspx</h2>
    <pre>
    &lt;daypilot:daypilotcalendar 
        &nbsp; id="DayPilotCalendar1"
        &nbsp; runat="server"
        &nbsp; DataStartField="Start"
        &nbsp; dataendfield="End"
        &nbsp; datatextfield="Name"
        &nbsp; datavaluefield="Id"&gt;
        &lt;/daypilot:daypilotcalendar&gt;</pre>
    <h2>
        ArrayListBindix.aspx.cs</h2>
    <pre>    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DayPilotCalendar1.DataSource = getData();
            DataBind();
        }
    }

    ArrayList getData()
    {
        ArrayList al = new ArrayList();

        CustomEvent ce = new CustomEvent();
        ce.Start = Convert.ToDateTime("15:30");
        ce.End = Convert.ToDateTime("16:30");
        ce.Name = "My event";
        ce.Id = "1";

        al.Add(ce);

        return al;

    }

    public class CustomEvent
    {
        private string name;
        private DateTime start;
        private DateTime end;
        private string id;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public DateTime Start
        {
            get { return start; }
            set { start = value; }
        }

        public DateTime End
        {
            get { return end; }
            set { end = value; }
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
    }</pre>
    <p>
        &nbsp;</p>
</asp:Content>

