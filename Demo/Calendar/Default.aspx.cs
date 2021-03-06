using System;
using System.Data;
using DayPilot.Web.Ui.Events;

public partial class _Default : System.Web.UI.Page
{
    private DataTable table;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["AllFeatures"] == null)
        {
            Session["AllFeatures"] = DataGeneratorCalendar.GetData();
        }
        table = (DataTable)Session["AllFeatures"];
        DayPilotCalendar1.DataSource = table;

        if (!IsPostBack)
        {
            DayPilotCalendar1.StartDate = firstDayOfWeek(DateTime.Now, DayOfWeek.Sunday);
            DataBind();
        }
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
        if (e.DataItem.Source != null)
        {
            string color = e.DataItem["color"] as string;
            if (!String.IsNullOrEmpty(color))
            {
                e.DurationBarColor = color;
            }
        }
        e.CssClass = "test";
    }

    protected void DayPilotCalendar1_OnEventMove(object sender, EventMoveEventArgs e)
    {
        #region Simulation of database update

        DataRow dr = table.Rows.Find(e.Id);
        if (dr != null)
        {
            dr["start"] = e.NewStart;
            dr["end"] = e.NewEnd;
            table.AcceptChanges();
        }

        #endregion

        DayPilotCalendar1.DataBind();
        DayPilotCalendar1.Update();
    }

    protected void DayPilotCalendar1_OnEventResize(object sender, EventResizeEventArgs e)
    {
        #region Simulation of database update

        DataRow dr = table.Rows.Find(e.Id);
        if (dr != null)
        {
            dr["start"] = e.NewStart;
            dr["end"] = e.NewEnd;
            table.AcceptChanges();
        }

        #endregion

        DayPilotCalendar1.DataBind();
        DayPilotCalendar1.Update();
    }

    protected void DayPilotCalendar1_OnTimeRangeSelected(object sender, TimeRangeSelectedEventArgs e)
    {
        #region Simulation of database update

        DataRow dr = table.NewRow();
        dr["start"] = e.Start;
        dr["end"] = e.End;
        dr["id"] = Guid.NewGuid().ToString();
        dr["name"] = "New event";

        table.Rows.Add(dr);
        table.AcceptChanges();
        #endregion

        DayPilotCalendar1.DataBind();
        DayPilotCalendar1.Update();

    }

    protected void DayPilotCalendar1_OnEventClick(object sender, EventClickEventArgs e)
    {
    }
}
