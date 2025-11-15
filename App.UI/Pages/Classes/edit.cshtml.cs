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
    [DisableRequestSizeLimit]
    public class editModel : PageModel
    {
        private readonly ClassManager classManager;
        private readonly WeekManager weekManager;
        private readonly TimeManager timeManager;
        private readonly ServiceManager serviceManager;
        private readonly int NumberOfWeeksAppearInMarkup = 5;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        [BindProperty]
        public Core.Entities.Classes CurrentClass { get; set; }

        public List<Weeks> Weeks { get; set; }
        public List<Time> TimeList { get; set; }
        public List<Services> ServiceList { get; set; }
        public List<Servants> ServantList { get; set; }
        public List<Served> ServedList { get; private set; }
        [BindProperty(SupportsGet =false)]
        public List<ServantWeeksDTO> ServantWeeksDTOs { get; set; }

        [BindProperty]
        public List<ServedWeeksDTO> ServedWeeksDTOs { get; set; }
        public editModel(ClassManager classManager, WeekManager weekManager, TimeManager timeManager, ServiceManager serviceManager)
        {
            this.classManager = classManager;
            this.weekManager = weekManager;
            this.timeManager = timeManager;
            this.serviceManager = serviceManager;
        }

        public void OnGet()
        {
            FillData();
        }

        //public void OnPost()
        //{
        //    var Result = classManager.UpdateClass(CurrentClass, ServantWeeksDTOs, ServedWeeksDTOs);

        //    TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
        //    TempData["Message"] = Result.IsSuccess ? "تم تحديث البيانات بنجاح" : Result.Error;
        //    FillData();

        //}
        public async Task<IActionResult> OnPostAsync()
        {
            var Result = await classManager.UpdateClassAsync(CurrentClass, ServantWeeksDTOs, ServedWeeksDTOs);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم تحديث البيانات بنجاح" : Result.Error;
            FillData();

            return Page();
        }

        public void FillData()
        {
            CurrentClass = classManager.GetClass(Id);
            Weeks = weekManager.GetWeeks(NumberOfWeeksAppearInMarkup);
            TimeList = timeManager.GetTimeList();
            ServiceList = serviceManager.GetServicesList();
            ServantList = CurrentClass.Servants?.ToList();
            ServedList = CurrentClass.Served?.ToList();
        }

        public string GetFormattedWeekDate(DateTime week, string Time)
        {
            return classManager.GetFormattedWeekDate(week, Time);
        }
    }

}
