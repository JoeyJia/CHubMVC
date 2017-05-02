using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace CHubCommon
{
    public class DateHelper
    {
        public static int GetWeekOfYear(DateTime t)
        {
            GregorianCalendar gc = new GregorianCalendar();
            return gc.GetWeekOfYear(t, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }
    }
}
