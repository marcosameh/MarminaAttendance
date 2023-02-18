using App.Core.Entities;
using App.Core.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages.Classes
{
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
        public editModel(ClassManager classManager,WeekManager  weekManager,TimeManager timeManager)
        {
            this.classManager = classManager;
            this.weekManager = weekManager;
            this.timeManager = timeManager;
        }
      
        public void OnGet()
        {
            CurrentClass=classManager.GetClass(Id);
            Weeks = weekManager.GetWeeks();
            TimeList=timeManager.GetTimeList();
        }
    }
}
