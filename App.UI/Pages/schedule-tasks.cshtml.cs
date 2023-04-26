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
                          Cron.Weekly(DayOfWeek.Wednesday, hour: 22, minute: 1));

        }
        public void AddNewWeek()
        {
            weekManager.AddNewWeek();
        }

    }
}
