using System;

namespace App.Core.Models
{
    public class BirthdayServedVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string Phone { get; set; }
        public string Photo { get; set; }
        public DateTime? Birthday { get; set; }
        public int Day { get; set; }
        public int Age { get; set; }

        public string PhotoPath
        {
            get
            {
                if (string.IsNullOrEmpty(Photo))
                {
                    return "/photos/served/default.jpg";
                }
                return "/photos/served/" + Photo;
            }
        }

        public string BirthdayFormatted => Birthday.HasValue ? Birthday.Value.ToString("dd/MM/yyyy") : string.Empty;

        public bool IsToday { get; set; }
        public bool IsYesterday { get; set; }
        public bool IsTomorrow { get; set; }

        public string DayStatus
        {
            get
            {
                if (IsToday) return "today";
                if (IsYesterday) return "yesterday";
                if (IsTomorrow) return "tomorrow";
                return "";
            }
        }

        public string DayStatusText
        {
            get
            {
                if (IsToday) return "النهارده";
                if (IsYesterday) return "امبارح";
                if (IsTomorrow) return "بكره";
                return "";
            }
        }
    }
}
