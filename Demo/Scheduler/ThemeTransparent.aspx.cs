using System;
using System.Data;
using DayPilot.Web.Ui.Events;
using DayPilot.Web.Ui.Events.Scheduler;

public partial class ThemeTransparent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DayPilotScheduler1.StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DayPilotScheduler1.Days = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);
            Label1.Text = DayPilotScheduler1.StartDate.ToString("MMMM yyyy");
            DayPilotScheduler1.DataSource = getData();
            DataBind();
        }
    }

    protected DataTable getData()
    {
        DataTable dt;
        dt = new DataTable();
        dt.Columns.Add("start", typeof(DateTime));
        dt.Columns.Add("end", typeof(DateTime));
        dt.Columns.Add("name", typeof(string));
        dt.Columns.Add("id", typeof(string));
        dt.Columns.Add("resource", typeof(string));
        dt.Columns.Add("color", typeof (string));

        DataRow dr;

        dr = dt.NewRow();
        dr["id"] = 0;
        dr["start"] = DateTime.Today;
        dr["end"] = DateTime.Today.AddDays(2);
        dr["name"] = "Event 1";
        dr["resource"] = "A";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 1;
        dr["start"] = DateTime.Today.AddDays(-1);
        dr["end"] = Convert.ToDateTime("17:00").AddDays(1);
        dr["name"] = "Event 2";
        dr["resource"] = "A";
        dr["color"] = "red";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 2;
        dr["start"] = DateTime.Today.AddDays(-7);
        dr["end"] = DateTime.Today.AddMonths(1);
        dr["name"] = "Event 3";
        dr["resource"] = "B";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 3;
        dr["start"] = DateTime.Today.AddMonths(-1);
        dr["end"] = DateTime.Today;
        dr["name"] = "Sales Dept. Meeting Once Again";
        dr["resource"] = "D";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 4;
        dr["start"] = Convert.ToDateTime("8:00");
        dr["end"] = Convert.ToDateTime("9:00");
        dr["name"] = "Event 4";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 5;
        dr["start"] = Convert.ToDateTime("22:00");
        dr["end"] = Convert.ToDateTime("6:00").AddDays(1);
        dr["name"] = "Event 5";
        dr["resource"] = "E";
        dt.Rows.Add(dr);


        dr = dt.NewRow();
        dr["id"] = 6;
        dr["start"] = Convert.ToDateTime("11:00");
        dr["end"] = Convert.ToDateTime("13:00");
        dr["name"] = "Event 6";
        dt.Rows.Add(dr);

        return dt;

    }

    protected void DayPilotScheduler1_BeforeEventRender(object sender, BeforeEventRenderEventArgs e)
    {
        string color = e.DataItem["color"] as string;
        if (!String.IsNullOrEmpty(color))
        {
            e.DurationBarColor = color;
        }
    }
}
