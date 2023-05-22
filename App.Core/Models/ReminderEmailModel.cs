using App.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models
{
    public class ReminderEmailModel
    {
        public List<Servants> ServantsWhoWillRecieveReminderEmails { get; set; }
        public List<Served> ServedNeedTobeRemembered { get; set; }

        public ReminderEmailModel(List<Servants> servants,List<Served> serveds)
        {
            ServantsWhoWillRecieveReminderEmails = servants;
            ServedNeedTobeRemembered= serveds;
        }

    }
}
