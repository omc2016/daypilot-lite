using System;
using System.Collections;

public partial class BindingArrayList : System.Web.UI.Page
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
        ce.Start = Convert.ToDateTime("15:30");
        ce.End = Convert.ToDateTime("16:30");
        ce.Name = "My event";
        ce.Id = "1";

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
}
