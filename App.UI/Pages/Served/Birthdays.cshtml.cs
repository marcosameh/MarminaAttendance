using App.Core.Managers;
using App.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace App.UI.Pages.Serveds
{
    [Authorize]
    public class BirthdaysModel : PageModel
    {
        private readonly ServedManager _servedManager;

        public List<BirthdayServedVM> BirthdayServed { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int Month { get; set; }

        public string MonthName { get; set; }

        public List<MonthItem> Months { get; set; } = new();

        public BirthdaysModel(ServedManager servedManager)
        {
            _servedManager = servedManager;
        }

        public async Task OnGetAsync()
        {
            // Default to current month if not specified
            if (Month == 0)
            {
                Month = DateTime.Now.Month;
            }

            // Populate months list
            PopulateMonths();

            // Get month name in Arabic
            MonthName = GetArabicMonthName(Month);

            // Get birthday served for the selected month
            BirthdayServed = await _servedManager.GetServedByBirthdayMonthAsync(Month);
        }

        private void PopulateMonths()
        {
            Months = new List<MonthItem>
            {
                new MonthItem { Value = 1, Name = "يناير" },
                new MonthItem { Value = 2, Name = "فبراير" },
                new MonthItem { Value = 3, Name = "مارس" },
                new MonthItem { Value = 4, Name = "ابريل" },
                new MonthItem { Value = 5, Name = "مايو" },
                new MonthItem { Value = 6, Name = "يونيو" },
                new MonthItem { Value = 7, Name = "يوليو" },
                new MonthItem { Value = 8, Name = "اغسطس" },
                new MonthItem { Value = 9, Name = "سبتمبر" },
                new MonthItem { Value = 10, Name = "اكتوبر" },
                new MonthItem { Value = 11, Name = "نوفمبر" },
                new MonthItem { Value = 12, Name = "ديسمبر" }
            };
        }

        private string GetArabicMonthName(int month)
        {
            return month switch
            {
                1 => "يناير",
                2 => "فبراير",
                3 => "مارس",
                4 => "ابريل",
                5 => "مايو",
                6 => "يونيو",
                7 => "يوليو",
                8 => "اغسطس",
                9 => "سبتمبر",
                10 => "اكتوبر",
                11 => "نوفمبر",
                12 => "ديسمبر",
                _ => ""
            };
        }
    }

    public class MonthItem
    {
        public int Value { get; set; }
        public string Name { get; set; }
    }
}
