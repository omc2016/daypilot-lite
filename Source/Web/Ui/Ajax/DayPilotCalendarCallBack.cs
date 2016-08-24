using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using DayPilot.Json;
using DayPilot.Web.Ui.Enums.Calendar;

namespace DayPilot.Web.Ui.Ajax
{
    /// <summary>
    /// CallBack shortcut class. Exposes properties that can be modified during CallBack update.
    /// </summary>
    public class DayPilotCalendarCallBack
    {
        private readonly DayPilotCalendar _calendar;
        internal DayPilotCalendarCallBack(DayPilotCalendar calendar)
        {
            _calendar = calendar;
        }


        /// <summary>
        /// Gets or sets the number of days to be displayed. Default is 1.
        /// </summary>
        public int Days
        {
            get
            {
                return _calendar.Days;
            }
            set
            {
                _calendar.Days = value;
            }
        }

        /// <summary>
        /// Gets or sets the first day to be shown. Default is DateTime.Today.
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                return _calendar.StartDate;
            }
            set
            {
                _calendar.StartDate = value;
            }
        }


        /// <summary>
        /// Sets or gets the view type (day, week, custom number of days, custom column set). If set to <see cref="ViewTypeEnum.Resources">ViewTypeEnum.Resources</see> and you use data binding you have to specify <see cref="DayPilotCalendar.DataColumnField">DataColumnField</see> property.
        /// </summary>
        public ViewTypeEnum ViewType
        {
            get
            {
                return _calendar.ViewType;
            }
            set
            {
                _calendar.ViewType = value;
            }
        }

        internal String GetHash()
        {
            JsonData data = new JsonData();
            data["days"] = Days;
            data["startDate"] = StartDate.ToString("s");
            data["viewType"] = ViewType.ToString();

            byte[] bytes = Encoding.ASCII.GetBytes(data.ToJson());
            return Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(bytes));
        }


    }

}
