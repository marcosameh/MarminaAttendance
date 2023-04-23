using App.Core.Managers;
using Hangfire;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages
{
    public class ScheduleTasksModel : PageModel
    {
        private readonly WeekManager weekManager;

        public ScheduleTasksModel(WeekManager weekManager)
        {
            
            this.weekManager = weekManager;
        }

       
        public void OnGet()
        {       
            RecurringJob.AddOrUpdate(() => AddNewWeek(),
                          Cron.Weekly(DayOfWeek.Saturday, hour: 16, minute: 35));

        }
        public void AddNewWeek()
        {
            weekManager.AddNewWeek();
        }

    }
}
