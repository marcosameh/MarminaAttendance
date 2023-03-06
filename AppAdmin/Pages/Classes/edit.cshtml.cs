using App.Core.Entities;
using App.Core.Managers;
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
        public App.Core.Entities.Classes CurrentClass { get; set; }

        public List<Weeks> Weeks { get; set; }
        public List<Time> TimeList { get; set; }
        [BindProperty]
        public Dictionary<int, List<int>> CheckedServantWeeks { get; set; }
        public editModel(ClassManager classManager,WeekManager  weekManager,TimeManager timeManager)
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
            var Result = classManager.UpdateClass(CurrentClass, CheckedServantWeeks);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم تحديث البيانات بنجاح" : Result.Error;
            FillData();

        }
        public void FillData()
        {
            CurrentClass = classManager.GetClass(Id);
            Weeks = weekManager.GetWeeks();
            TimeList = timeManager.GetTimeList();
        }
    }
}
