using App.Core.Entities;
using App.Core.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Core.Common;
using System.Runtime.InteropServices;

namespace App.Core.Managers
{
    public class ClassManager
    {
        private readonly MarminaAttendanceContext _dbContext;

        public ClassManager(MarminaAttendanceContext context)
        {
            _dbContext = context;
        }
        public List<ClassVM> GetClasses()
        {

            return _dbContext.Classes.Include(x => x.Time).Select(x => new ClassVM
            {
                Id = x.Id,
                Name = x.Name,
                Intercessor = x.Intercessor,
                Time = x.Time.Time1
            }).ToList();
        }
        public Classes GetClass(int id)
        {
            return _dbContext.Classes.Where(x => x.Id == id).Include(x => x.Servants).ThenInclude(x => x.ServantWeek).FirstOrDefault();
        }
        public Result EditClass(Classes ClassData, Dictionary<int, List<int>> SelectedServantWeeks)
        {
            // Retrieve the existing class and update its properties
            var existingClass = _dbContext.Classes
                .Include(c => c.Servants).ThenInclude(x => x.ServantWeek)
                .SingleOrDefault(c => c.Id == ClassData.Id);

            if (existingClass == null)
            {
                // Return an error if the class does not exist
                return Result.Fail("Class not found");
            }

            existingClass.Name = ClassData.Name;
            existingClass.Intercessor = ClassData.Intercessor;
            existingClass.TimeId = ClassData.TimeId;

            // Update the ServantWeek records for each servant
            foreach (var servant in existingClass.Servants)
            {
                var ExistServantWeeks = servant.ServantWeek.ToList();

                var selectedWeeks = SelectedServantWeeks[servant.Id];


                //if firsttime to add ServantWeek
                if (ExistServantWeeks.Any() == false && selectedWeeks.Any())
                {
                    foreach (var weekId in selectedWeeks)
                    {
                        servant.ServantWeek.Add(new ServantWeek { ServentId = servant.Id, WeekId = weekId });
                    }
                    return Result.Ok("Class updated successfully");
                }
                if (ExistServantWeeks.Any() && selectedWeeks.Any() == false)
                {
                    foreach (var ServantWeeks in ExistServantWeeks)
                    {
                        servant.ServantWeek.Remove(ServantWeeks);

                    }

                    return Result.Ok("Class updated successfully");
                }
                //if (ExistServantWeeks.Any() && selectedWeeks.Any())
                //{
                //    foreach (var W in collection)
                //    {

                //    }
                //}


            }
            _dbContext.SaveChanges();
            return Result.Ok("Class updated successfully");


        }
    }
}
