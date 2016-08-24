using System;
using System.Data;
using DayPilot.Web.Ui.Events;
using DayPilot.Web.Ui.Events.Scheduler;

public partial class SchedulerTimeline : System.Web.UI.Page
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

    protected static DataTable getData()
    {
        DataTable dt;
        dt = new DataTable();
        dt.Columns.Add("start", typeof(DateTime));
        dt.Columns.Add("end", typeof(DateTime));
        dt.Columns.Add("id", typeof(string));
        dt.Columns.Add("resource", typeof(string));
        dt.Columns.Add("empty", typeof(string));

        DataRow dr;

        DateTime start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

        dr = dt.NewRow();
        dr["id"] = 0;
        dr["start"] = start;
        dr["end"] = start.AddDays(2);
        dr["resource"] = "A";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 1;
        dr["start"] = start.AddDays(2);
        dr["end"] = start.AddDays(5);
        dr["resource"] = "B";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 2;
        dr["start"] = start.AddDays(5);
        dr["end"] = start.AddDays(7);
        dr["resource"] = "C";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 3;
        dr["start"] = start.AddDays(7);
        dr["end"] = start.AddDays(8);
        dr["resource"] = "D";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 4;
        dr["start"] = start.AddDays(8);
        dr["end"] = start.AddDays(9);
        dr["resource"] = "E";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 5;
        dr["start"] = start;
        dr["end"] = start.AddDays(9);
        dr["resource"] = "F";
        dt.Rows.Add(dr);

        return dt;

    }

    protected void DayPilotScheduler1_OnBeforeEventRender(object sender, BeforeEventRenderEventArgs e)
    {
        e.InnerHTML = String.Empty;
        e.BackgroundColor = "#CA2A50";
    }
}
