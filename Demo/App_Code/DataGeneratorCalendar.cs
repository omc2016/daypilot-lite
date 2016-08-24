using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DataGeneratorCalendar
/// </summary>
public class DataGeneratorCalendar
{

    public static DataTable GetData()
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
        dr["start"] = Convert.ToDateTime("10:50").AddDays(1);
        dr["end"] = Convert.ToDateTime("15:55").AddDays(1);
        dr["name"] = "Event 1";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 1;
        dr["start"] = Convert.ToDateTime("16:00");
        dr["end"] = Convert.ToDateTime("17:00");
        dr["name"] = "Event 2";
        dr["color"] = "red";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 2;
        dr["start"] = Convert.ToDateTime("16:15");
        dr["end"] = Convert.ToDateTime("18:45");
        dr["name"] = "Event 3";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 3;
        dr["start"] = Convert.ToDateTime("10:30").AddDays(2);
        dr["end"] = Convert.ToDateTime("12:30").AddDays(2);
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

        dr = dt.NewRow();
        dr["id"] = 7;
        dr["start"] = Convert.ToDateTime("14:00").AddDays(-1);
        dr["end"] = Convert.ToDateTime("17:00").AddDays(-1);
        dr["name"] = "Event 7";
        dt.Rows.Add(dr);

        dt.PrimaryKey = new DataColumn[] { dt.Columns["id"] };

        return dt;

    }

}