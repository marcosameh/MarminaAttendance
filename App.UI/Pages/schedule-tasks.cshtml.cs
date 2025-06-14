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
        private readonly IConfiguration configuration;

        public ScheduleTasksModel(WeekManager weekManager,
            ClassManager classManager, 
            EmailManager mailManager, 
            IViewRenderService viewRenderService,
            IConfiguration configuration)
        {

            this.weekManager = weekManager;
            this.classManager = classManager;
            this._mailManager = mailManager;
            this.viewRenderService = viewRenderService;
            this.configuration = configuration;
        }


        public void OnGet()
        {
            RecurringJob.AddOrUpdate(() => AddNewWeek(), Cron.Weekly(DayOfWeek.Thursday, hour: 0, minute: 1));
            RecurringJob.AddOrUpdate(() => SendReminderEmailsAsync(), Cron.Monthly(1));
            RecurringJob.AddOrUpdate(() => SendBirthdayEmailsAsync(), Cron.Daily());


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
                var MsgCC = new List<string>();
                if (configuration["AppName"]== "مدارس الاحد")
                {
                    MsgCC.Add("Hany.motie@gmail.com");
                }
                var emailContent = await viewRenderService.RenderToStringAsync("ReminderEmail", reminderEmail);
                if (MsgTo != null && MsgTo.Any())
                {
                    _mailManager.SendEmail(emailSubject, MsgTo, MsgCC.ToArray(), emailContent);
                }

            }
        }
        public async Task SendBirthdayEmailsAsync()
        {
            var emailSubject = $"🎉 أعياد ميلاد اليوم - {DateTime.Now:dd/MM} 🎂";

            var birthdayEmails = classManager.GetServedNeedToBeRememberedforBithday();
            foreach (var birthdayEmail in birthdayEmails)
            {
                var MsgTo = birthdayEmail.Servants.Select(x => x.Email).ToArray();
                var emailContent = await viewRenderService.RenderToStringAsync("BirthdayEmail", birthdayEmail);
                var MsgCC = new List<string>();
                if (configuration["AppName"] == "مدارس الاحد")
                {
                    MsgCC.Add("Hany.motie@gmail.com");
                }
                if (MsgTo != null && MsgTo.Any())
                {
                    _mailManager.SendEmail(emailSubject, MsgTo, MsgCC.ToArray(), emailContent);
                }

            }
        }

    }
}
