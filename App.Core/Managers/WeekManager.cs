using App.Core.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Managers
{
    public class WeekManager
    {
        private readonly MarminaAttendanceContext context;
        
        public WeekManager(MarminaAttendanceContext context)
        {
            this.context = context;
           
        }
        public List<Weeks> GetWeeks(int NumberOfWeeksAppearInMarkup)
        {
            return context.Weeks.OrderByDescending(x=>x.Id).Take(NumberOfWeeksAppearInMarkup).AsNoTracking().ToList();
        }

        public void AddNewWeek()
        {
            var today = DateTime.Today;
            context.Weeks.Add(new Weeks { Date = today });
            context.SaveChanges();
        }
    }
}
