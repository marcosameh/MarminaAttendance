using App.Core.Infrastrcuture;
using App.Core.Managers;
using AppCore.Infrastructure;
using Hangfire;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages
{
    public class ScheduleTasksModel : PageModel
    {
        private readonly WeekManager weekManager;
        private readonly ClassManager classManager;
        private readonly EmailManager _mailManager;
        private readonly IViewRenderService viewRenderService;

        public ScheduleTasksModel(WeekManager weekManager,ClassManager classManager,EmailManager mailManager, IViewRenderService viewRenderService)
        {
            
            this.weekManager = weekManager;
            this.classManager = classManager;
            this._mailManager = mailManager;
            this.viewRenderService = viewRenderService;
        }

       
        public void OnGet()
        {       
            RecurringJob.AddOrUpdate(() => AddNewWeek(),Cron.Weekly(DayOfWeek.Wednesday, hour: 22, minute: 1));
            RecurringJob.AddOrUpdate(() => SendReminderEmailsAsync(), Cron.Monthly(1));


        }
        public void AddNewWeek()
        {
            weekManager.AddNewWeek();
        }
        public async Task SendReminderEmailsAsync()
        {
            var emailSubject = $"{DateTime.Now.AddMonths(-1).Month}افتقاد شهر";
            var reminderEmails = classManager.GetServedNeedToBeRemembered();
            foreach (var reminderEmail in reminderEmails)
            {
                var MsgTo = reminderEmail.ServantsWhoWillRecieveReminderEmails.Select(x => x.Email).ToArray();
                var emailContent = await viewRenderService.RenderToStringAsync("ReminderEmail", reminderEmail);
                
                _mailManager.SendEmail(emailSubject,MsgTo,emailContent);
            }
        }


    }
}
