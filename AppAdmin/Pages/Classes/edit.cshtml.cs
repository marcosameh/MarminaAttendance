using App.Core.Entities;
using App.Core.Enums;
using App.Core.Managers;
using App.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol.Core.Types;
using System.Globalization;

namespace App.UI.Pages.Classes
{
    [Authorize]
    public class editModel : PageModel
    {
        private readonly ClassManager classManager;
        private readonly WeekManager weekManager;
        private readonly TimeManager timeManager;
        private readonly int NumberOfWeeksAppearInMarkup = 5;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        [BindProperty]
        public Core.Entities.Classes CurrentClass { get; set; }

        public List<Weeks> Weeks { get; set; }
        public List<Time> TimeList { get; set; }
        public List<Servants> ServantList { get; set; }
        public List<Served> ServedList { get; private set; }
        [BindProperty]
        public List<ServantWeeksDTO> ServantWeeksDTOs { get; set; }

        [BindProperty]
        public List<ServedWeeksDTO> ServedWeeksDTOs { get; set; }
        public editModel(ClassManager classManager, WeekManager weekManager, TimeManager timeManager)
        {
            this.classManager = classManager;
            this.weekManager = weekManager;
            this.timeManager = timeManager;
        }

        public void OnGet()
        {
            FillData();
        }
        public void OnPost()
        {
            var Result = classManager.UpdateClass(CurrentClass, ServantWeeksDTOs, ServedWeeksDTOs);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم تحديث البيانات بنجاح" : Result.Error;
            FillData();

        }
        public void FillData()
        {
            CurrentClass = classManager.GetClass(Id);
            Weeks = weekManager.GetWeeks(5);
            TimeList = timeManager.GetTimeList();
            ServantList = CurrentClass.Servants?.ToList();
            ServedList = CurrentClass.Served?.ToList();
        }

        public string GetFormattedWeekDate(DateTime week,string Time)
        {
            ServiceTime serviceTime;
            if (!Enum.TryParse(Time.Replace(" ", ""), out serviceTime))
            {
                throw new ArgumentException($"Invalid value for 'time': {Time}");
            }
            string FormatedDate = (serviceTime) switch
            {
                ServiceTime.الخميس => week.ToString("dd/MM/yyyy"),
                ServiceTime.الجمعةصباحا => week.AddDays(1).ToString("dd/MM/yyyy"),
                ServiceTime.الجمعةمساء => week.AddDays(1).ToString("dd/MM/yyyy"),
                ServiceTime.السبت => week.AddDays(2).ToString("dd/MM/yyyy"),
                _ => week.ToString("dd/MM/yyyy"),

            };
            return FormatedDate;
        }
    }

}
