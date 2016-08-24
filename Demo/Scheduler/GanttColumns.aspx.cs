using System;
using System.Data;
using DayPilot.Web.Ui.Events;
using DayPilot.Web.Ui.Events.Scheduler;

public partial class GanttColumns : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            DayPilotScheduler1.RowHeaderColumnWidths = "150, 100";
            DayPilotScheduler1.StartDate = DateTime.Today;
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

        dr = dt.NewRow();
        dr["id"] = 1;
        dr["start"] = DateTime.Today;
        dr["end"] = DateTime.Today.AddHours(2);
        dr["name"] = "Event 1";
        dr["resource"] = "A";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 2;
        dr["start"] = DateTime.Today.AddHours(3);
        dr["end"] = DateTime.Today.AddHours(10);
        dr["name"] = "Event 2";
        dr["resource"] = "A";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 3;
        dr["start"] = DateTime.Today.AddHours(5);
        dr["end"] = DateTime.Today.AddHours(17);
        dr["name"] = "Event 3";
        dr["resource"] = "B";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 4;
        dr["start"] = Convert.ToDateTime("8:00");
        dr["end"] = Convert.ToDateTime("9:00");
        dr["name"] = "Event 4";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 5;
        dr["start"] = DateTime.Today.AddHours(10);
        dr["end"] = DateTime.Today.AddHours(23);
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

    protected void DayPilotScheduler1_BeforeResHeaderRender(object sender, BeforeResHeaderRenderEventArgs e)
    {
        DateTime start = (DateTime) e.DataItem["start"];
        DateTime end = (DateTime) e.DataItem["end"];
        e.InnerHTML = String.Format("<div style='padding: 0px 4px 0px 2px;'>{0}</div>", e.Name);
        e.Columns[0].InnerHTML = String.Format("<div style='text-align:right; padding: 0px 4px 0px 2px;'>{0}</div>", end - start);
    }

    protected void DayPilotScheduler1_RowHeaderColumnWidthChanged(object sender, HeaderColumnWidthChangedEventArgs e)
    {
        DayPilotScheduler1.DataSource = getData();
        DataBind();

    }
}
