using System;
using System.Globalization;
using System.Text;

namespace AppCore.Utilities
{
    public static class DateUtilities
    {
        public static string TimeSpanToFriendlyString(this TimeSpan ts)
        {
            if (ts.Days < 0)
                return "N/A";

            StringBuilder sb = new StringBuilder();
            int years = ts.Days / 365; 
            int months = (ts.Days % 365) / 30; 
            int weeks = ((ts.Days % 365) % 30) / 7;
            
            if (years > 0)
            {
                sb.Append(years.ToString() + " year" + (years > 1 ? "s" : string.Empty) + ", ");
            }
            if (months > 0)
            {
                sb.Append(months.ToString() + " month" + (months > 1 ? "s" : string.Empty) + ", ");
            }
            if (years == 0)
            {
                if (weeks > 0)
                {
                    sb.Append(weeks.ToString() + " week" + (weeks > 1 ? "s" : string.Empty) + ", ");
                }
            }

            return sb.Remove(sb.Length - 2, 2).ToString();

        }

        public static DateTime ToCultureInVariant(this string date)
        {
            if (DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                       DateTimeStyles.None, out DateTime dayStartingDate))
            {
                return dayStartingDate;
            }
             
            if (DateTime.TryParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime monthStartingDate))
            {
                return monthStartingDate;
            }


            if (DateTime.TryParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                      DateTimeStyles.None, out DateTime dotteddayStartingDate))
            {
                return dotteddayStartingDate;
            }
           
                return default;
        }

        public static bool IsEmpty(this DateTime dateTime)
        {
           return dateTime == default;
        }

        public static string ToMonthName(this DateTime value)
        {
            return value.ToString("MMMM", CultureInfo.InvariantCulture);
        }

        public static string ToMonthName(this int value)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(value);
        }
    }
}