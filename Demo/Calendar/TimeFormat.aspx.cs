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

public partial class TimeFormat : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (DropDownList1.SelectedValue)
        {
            case "12": 
                DayPilotCalendar1.TimeFormat = DayPilot.Web.Ui.Enums.TimeFormat.Clock12Hours;
                break;
            case "24":
                DayPilotCalendar1.TimeFormat = DayPilot.Web.Ui.Enums.TimeFormat.Clock24Hours;
                break;
        }
        
    }
}
