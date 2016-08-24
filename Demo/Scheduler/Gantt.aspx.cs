using System;
using System.Data;
using DayPilot.Utils;

public partial class Gantt : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DayPilotScheduler1.StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DayPilotScheduler1.Days = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);
            Label1.Text = DateTime.Today.ToShortDateString();
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

        DataRow dr;

        DateTime start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

        dr = dt.NewRow();
        dr["id"] = 1;
        dr["start"] = start;
        dr["end"] = start.AddDays(2);
        dr["name"] = "Event 1";
        dr["resource"] = "A";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 2;
        dr["start"] = start.AddDays(2);
        dr["end"] = start.AddDays(4);
        dr["name"] = "Event 2";
        dr["resource"] = "A";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 3;
        dr["start"] = start.AddDays(2);
        dr["end"] = start.AddMonths(1);
        dr["name"] = "Event 3";
        dr["resource"] = "B";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 4;
        dr["start"] = start.AddDays(5);
        dr["end"] = start.AddDays(15);
        dr["name"] = "Event 4";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 5;
        dr["start"] = start.AddDays(10);
        dr["end"] = start.AddDays(20);
        dr["name"] = "Event 5";
        dr["resource"] = "E";
        dt.Rows.Add(dr);


        dr = dt.NewRow();
        dr["id"] = 6;
        dr["start"] = start.AddDays(1);
        dr["end"] = start.AddDays(25);
        dr["name"] = "Event 6";
        dt.Rows.Add(dr);

        return dt;

    }

}
