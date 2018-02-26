using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Shop.Web.Core.Helper
{
    public static class DateTimeHelper
    {
        public static string ToFullString(this DateTime day)
        {
            return string.Format("Ngày {0} tháng {1} năm {2}", day.Day, day.Month, day.Year);
        }

        public static DateTime StartOfDay(this DateTime source)
        {
            return Convert.ToDateTime(source.ToShortDateString());
        }

        public static DateTime EndOfDay(this DateTime source)
        {
            return Convert.ToDateTime(source.ToShortDateString() + " 11:59 PM");
        }

        public static int Subtract(DateTime from, DateTime to)
        {
            TimeSpan diffResult = to.Subtract(from);
            if (diffResult.Days == 0)
                return 1;
            else
                return diffResult.Days;
        }

        public static int GetYearOld(DateTime birthday)
        {
            return DateTime.Today.Year - birthday.Year;
        }

        public static string MonthOfYear(this int month)
        {
            string result = string.Empty;
            switch (month)
            {
                case 1:
                    result = "Jan";
                    break;
                case 2:
                    result = "Feb";
                    break;
                case 3:
                    result = "Mar";
                    break;
                case 4:
                    result = "Apr";
                    break;
                case 5:
                    result = "May";
                    break;
                case 6:
                    result = "Jun";
                    break;
                case 7:
                    result = "Jul";
                    break;
                case 8:
                    result = "Aug";
                    break;
                case 9:
                    result = "Sep";
                    break;
                case 10:
                    result = "Oct";
                    break;
                case 11:
                    result = "Nov";
                    break;
                case 12:
                    result = "Dec";
                    break;
            }
            return result;
        }

        public static IEnumerable<SelectListItem> GetMonths()
        {
            var months = new List<SelectListItem>();
            for (int i = 1; i <= 12; i++)
                months.Add(new SelectListItem() {Text = i.MonthOfYear(), Value = i.ToString()});
            return months;
        }

        /// <summary>
        /// GetDayOfWeek
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetDayOfWeek(string key)
        {
            string result = "";
            switch (key)
            {
                case "Monday":
                    result = "Mon";
                    break;
                case "Tuesday":
                    result = "Tue";
                    break;
                case "Wednesday":
                    result = "Wed";
                    break;
                case "Thursday":
                    result = "Thur";
                    break;
                case "Friday":
                    result = "Fri";
                    break;
                case "Saturday":
                    result = "Sat";
                    break;
                case "Sunday":
                    result = "Sun";
                    break;
            }
            return result;
        }

        public static IList<SelectListItem> DayOfWeeks()
        {
            return new List<SelectListItem>
            {
                new SelectListItem() {Text = "Thứ 2", Value = "Monday"},
                new SelectListItem() {Text = "Thứ 3", Value = "Tuesday"},
                new SelectListItem() {Text = "Thứ 4", Value = "Wednesday"},
                new SelectListItem() {Text = "Thứ 5", Value = "Thursday"},
                new SelectListItem() {Text = "Thứ 6", Value = "Friday"},
                new SelectListItem() {Text = "Thứ 7", Value = "Saturday"},
                new SelectListItem() {Text = "Chủ nhật", Value = "Sunday"},
            };
        }

      

        public static DateTime ConvertToShortDay(string input)
        {
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "MM/dd/yyyy HH:mm";
            dtfi.DateSeparator = "/";

            return Convert.ToDateTime(Regex.Replace(input,
                @"\b(?<day>\d{1,2})/(?<month>\d{1,2})/(?<year>\d{2,4})\b",
                "${month}/${day}/${year}"), dtfi);
        }
        public static DateTime ConvertToLongDay(string input)
        {
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "MM/dd/yyyy HH:mm";
            dtfi.DateSeparator = "/";

            return Convert.ToDateTime(Regex.Replace(input,
                @"\b(?<day>\d{1,2})/(?<month>\d{1,2})/(?<year>\d{2,4}) (?<hour>\d{1,2}):(?<minute>\d{1,2})\b",
                "${month}/${day}/${year} ${hour}:${minute}"), dtfi);
        }


        public static string TimeAgo(DateTime dateTime)
        {
            string result = string.Empty;
            var timeSpan = DateTime.Now.Subtract(dateTime);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} giây trước", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1
                    ? String.Format("khoản {0} phút trước", timeSpan.Minutes)
                    : "cách đây 1 phút";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1
                    ? String.Format("khoản {0} giờ trước", timeSpan.Hours)
                    : "khoản 1 giờ trước";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1
                    ? String.Format("khoản {0} ngày trước", timeSpan.Days)
                    : "ngày hôm qua";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30
                    ? String.Format("khoản {0} tháng trước", timeSpan.Days/30)
                    : "khoản 1 tháng trước";
            }
            else
            {
                result = timeSpan.Days > 365
                    ? String.Format("khoản {0} năm trước", timeSpan.Days/365)
                    : "khoản 1 năm trước";
            }

            return result;
        }
    }
}
