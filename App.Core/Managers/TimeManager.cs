using App.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Managers
{
    public class TimeManager
    {
        private readonly MarminaAttendanceContext context;

        public TimeManager(MarminaAttendanceContext context)
        {
            this.context = context;
        }
        public List<Time> GetTimeList()
        {
            return context.Time.AsNoTracking().ToList(); 
        }
    }
}
