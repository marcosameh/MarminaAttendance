using App.Core.Entities;

namespace App.Core.Models
{
    public class ReminderEmailModel
    {
        public List<Servants> ServantsWhoWillRecieveReminderEmails { get; set; }
        public List<string> ServiceAdminEmails { get; set; }
        public List<Served> ServedNeedTobeRemembered { get; set; }

        public ReminderEmailModel(List<Servants> servants, List<Served> serveds, List<string> serviceadminemails)
        {
            ServantsWhoWillRecieveReminderEmails = servants;
            ServedNeedTobeRemembered = serveds;
            ServiceAdminEmails=serviceadminemails;
        }

    }
}
