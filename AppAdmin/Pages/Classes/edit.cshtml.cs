using App.Core.Entities;
using App.Core.Managers;
using App.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages.Classes
{
    [Authorize]
    public class editModel : PageModel
    {
        private readonly ClassManager classManager;
        private readonly WeekManager weekManager;
        private readonly TimeManager timeManager;

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
            Weeks = weekManager.GetWeeks();
            TimeList = timeManager.GetTimeList();
            ServantList = CurrentClass.Servants?.ToList();
            ServedList= CurrentClass.Served?.ToList();
        }
    }
   
}
