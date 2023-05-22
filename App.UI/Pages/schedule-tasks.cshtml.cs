using App.Core.Infrastrcuture;
using App.Core.Managers;
using Hangfire;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages
{
    public class ScheduleTasksModel : PageModel
    {
        private readonly WeekManager weekManager;
        private readonly ClassManager classManager;
        private readonly EmailManager emailManager;

        public ScheduleTasksModel(WeekManager weekManager,ClassManager classManager,EmailManager emailManager)
        {
            
            this.weekManager = weekManager;
            this.classManager = classManager;
            this.emailManager = emailManager;
        }

       
        public void OnGet()
        {       
            RecurringJob.AddOrUpdate(() => AddNewWeek(),Cron.Weekly(DayOfWeek.Wednesday, hour: 22, minute: 1));
            RecurringJob.AddOrUpdate(() => SendReminderEmails(), Cron.Monthly(1));


        }
        public void AddNewWeek()
        {
            weekManager.AddNewWeek();
        }
        

    }
}
