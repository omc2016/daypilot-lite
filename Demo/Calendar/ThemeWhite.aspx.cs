using System;
using System.Data;

public partial class ThemeWhite : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DayPilotCalendar1.StartDate = firstDayOfWeek(DateTime.Now, DayOfWeek.Sunday);
            DayPilotCalendar1.DataSource = getData();
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
        dt.Columns.Add("color", typeof(string));

        DataRow dr;

        dr = dt.NewRow();
        dr["id"] = 0;
        dr["start"] = Convert.ToDateTime("13:30");
        dr["end"] = Convert.ToDateTime("16:00");
        dr["name"] = "Event 1";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 1;
        dr["start"] = Convert.ToDateTime("16:00").AddDays(1);
        dr["end"] = Convert.ToDateTime("17:00").AddDays(1);
        dr["name"] = "Event 2";
        dr["color"] = "red";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 2;
        dr["start"] = Convert.ToDateTime("9:15").AddDays(1);
        dr["end"] = Convert.ToDateTime("12:45").AddDays(1);
        dr["name"] = "Event 3";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 3;
        dr["start"] = Convert.ToDateTime("16:30");
        dr["end"] = Convert.ToDateTime("17:30");
        dr["name"] = "Sales Dept. Meeting Once Again";
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
        dt.Rows.Add(dr);


        dr = dt.NewRow();
        dr["id"] = 6;
        dr["start"] = Convert.ToDateTime("11:00");
        dr["end"] = Convert.ToDateTime("13:00");
        dr["name"] = "Event 6";
        dt.Rows.Add(dr);

        return dt;

    }

    /// <summary>
    /// Gets the first day of a week where day (parameter) belongs. weekStart (parameter) specifies the starting day of week.
    /// </summary>
    /// <returns></returns> 
    private static DateTime firstDayOfWeek(DateTime day, DayOfWeek weekStarts)
    {
        DateTime d = day;
        while (d.DayOfWeek != weekStarts)
        {
            d = d.AddDays(-1);
        }

        return d;
    }

    protected void DayPilotCalendar1_BeforeEventRender(object sender, DayPilot.Web.Ui.Events.Calendar.BeforeEventRenderEventArgs e)
    {
        string color = e.DataItem["color"] as string;
        if (!String.IsNullOrEmpty(color))
        {
            e.DurationBarColor = color;
        }
    }
}
