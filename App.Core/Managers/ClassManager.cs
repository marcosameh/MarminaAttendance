using App.Core.Entities;
using App.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace App.Core.Managers
{
    public class ClassManager
    {
        private readonly MarminaAttendanceContext context;

        public ClassManager(MarminaAttendanceContext context)
        {
            this.context = context;
        }
        public List<ClassVM> GetClasses()
        {

            return context.Classes.Include(x => x.Time).Select(x => new ClassVM
            {
                Id = x.Id,
                Name = x.Name,
                Intercessor = x.Intercessor,
                Time = x.Time.Time1
            }).ToList();
        }
        public Classes GetClass(int id)
        {
            return context.Classes.Where(x => x.Id == id).Include(x => x.Servants).ThenInclude(x => x.ServantWeek).FirstOrDefault();
        }

    }
}
