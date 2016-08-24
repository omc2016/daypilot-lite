using System;
using System.Collections.Generic;
using System.Text;
using DayPilot.Web.Ui.Enums.Calendar;

namespace DayPilot.Utils
{
    internal class ViewTypeParser
    {
        internal static ViewTypeEnum Parse(string input)
        {
            switch (input.ToUpper())
            {
                case "DAY":
                    return ViewTypeEnum.Day;
                case "DAYS":
                    return ViewTypeEnum.Days;
                case "WEEK":
                    return ViewTypeEnum.Week;
                case "WORKWEEK":
                    return ViewTypeEnum.WorkWeek;
                default:
                    throw new ArgumentException("Unrecognized ViewTypeEnum value.");

            }
        }
    }
}
