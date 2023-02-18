using App.Core.Entities;
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
        public List<Weeks> GetWeeks()
        {
            return context.Weeks.OrderByDescending(x=>x.Id).Take(5).ToList();
        }
    }
}
