using System;
using System.Data;
using DayPilot.Web.Ui.Events;
using DayPilot.Web.Ui.Events.Scheduler;

public partial class SchedulerMonth : System.Web.UI.Page
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

        DateTime start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

        dr = dt.NewRow();
        dr["id"] = 0;
        dr["start"] = start.AddDays(1);
        dr["end"] = start.AddDays(5);
        dr["name"] = "Event 1";
        dr["resource"] = "A";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 1;
        dr["start"] = start.AddDays(2);
        dr["end"] = start.AddDays(10);
        dr["name"] = "Event 2";
        dr["resource"] = "A";
        dr["color"] = "#00aa00";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 2;
        dr["start"] = start.AddDays(7);
        dr["end"] = start.AddDays(15);
        dr["name"] = "Event 3";
        dr["color"] = "#cc0000";
        dr["resource"] = "B";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 3;
        dr["start"] = start.AddDays(3);
        dr["end"] = start.AddDays(25);
        dr["name"] = "Sales Dept. Meeting Once Again";
        dr["color"] = "#0000cc";
        dr["resource"] = "D";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 4;
        dr["start"] = start.AddDays(1);
        dr["end"] = start.AddDays(10);
        dr["name"] = "Event 4";
        dr["resource"] = "I";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 5;
        dr["start"] = start.AddDays(4);
        dr["end"] = start.AddDays(14);
        dr["name"] = "Event 5";
        dr["resource"] = "E";
        dt.Rows.Add(dr);


        dr = dt.NewRow();
        dr["id"] = 6;
        dr["start"] = start.AddDays(3);
        dr["end"] = start.AddDays(7);
        dr["name"] = "Event 6";
        dr["resource"] = "F";
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
