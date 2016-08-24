using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DayPilot.Web.Ui;
using DayPilot.Web.Ui.Enums;

public partial class NonBusinessHours : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
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
        ce.Start = Convert.ToDateTime("6:30");
        ce.End = Convert.ToDateTime("19:30");
        ce.Name = "My event";
        ce.Id = "1";

        al.Add(ce);

        ce = new CustomEvent();
        ce.Start = Convert.ToDateTime("9:30");
        ce.End = Convert.ToDateTime("13:30");
        ce.Name = "My event 2";
        ce.Id = "2";

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
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (DropDownList1.SelectedValue)
        {
                
            case "hide":
                DayPilotCalendar1.HeightSpec = HeightSpecEnum.BusinessHoursNoScroll;
                break;
            case "scroll":
                DayPilotCalendar1.HeightSpec = HeightSpecEnum.BusinessHours;
                break;
            case "show":
                DayPilotCalendar1.HeightSpec = HeightSpecEnum.Full;
                break;
        }
    }
}
