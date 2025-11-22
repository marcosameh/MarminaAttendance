using App.Core.Entities;
using App.Core.Enums;
using App.Core.Models;
using AppCore.Common;
using Microsoft.EntityFrameworkCore;

namespace App.Core.Managers
{
    public class ClassManager
    {
        private readonly MarminaAttendanceContext _context;
        private readonly CurrentUserManager _currentUserManager;
        private int NumberOfWeeksAppearInMarkup = 5;

        public ClassManager(MarminaAttendanceContext context,
            CurrentUserManager currentUserManager)
        {
            _context = context;
            _currentUserManager = currentUserManager;

        }


        public async Task<IQueryable<ClassVM>> GetFilteredClassesQueryAsync()
        {
            var query = _context.Classes.Include(x => x.Time).Include(x => x.Service).AsQueryable();
            var currentServant = await _currentUserManager.GetCurrentServantAsync();

            if (currentServant != null && currentServant.ServiceId.HasValue)
            {
                query = query.Where(c => c.ServiceId == currentServant.ServiceId);
            }
            else if (currentServant != null && currentServant.ClassId.HasValue)
            {
                query = query.Where(c => c.Id == currentServant.ClassId);
            }

            return query
                .OrderBy(x => x.Name)
                .Select(x => new ClassVM
                {
                    Id = x.Id,
                    Name = x.Name,
                    Intercessor = x.Intercessor,
                    Time = x.Time.Time1,
                    ServiceName = x.Service != null ? x.Service.Name : ""
                });
        }
        public List<ClassVM> GetClasses()
        {

            return _context.Classes
                .Include(x => x.Time)
                .Include(x => x.Service)
                .OrderBy(x => x.Name)
                .Select(x => new ClassVM
                {
                    Id = x.Id,
                    Name = x.Name,
                    Intercessor = x.Intercessor,
                    Time = x.Time.Time1,
                    ServiceName = x.Service != null ? x.Service.Name : ""
                }).ToList();
        }

        public Result AddClass(Classes classData)
        {
            try
            {
                _context.Classes.Add(classData);
                _context.SaveChanges();
                return Result.Ok(string.Format(classData.Name, "{0}: بنجاح تم اضافة فصل"));
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.InnerException.Message);
            }

        }

        public Result DeleteClass(int Id)
        {
            try
            {
                var existClass = _context.Classes.Find(Id);
                if (existClass == null)
                {
                    Result.Fail("تم مسح الفصل بالفعل");
                }
                _context.Classes.Remove(existClass);
                _context.SaveChanges();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.InnerException.Message);
            }
        }
        public Classes GetClass(int id)
        {
            return _context.Classes.Where(x => x.Id == id).Include(x => x.Servants.OrderBy(x => x.Name)).ThenInclude(x => x.ServantWeek).
                Include(x => x.Served.OrderBy(x => x.Name)).ThenInclude(x => x.ServedWeeks).Include(x => x.Time).AsSplitQuery().AsNoTracking().FirstOrDefault();

        }

        public async Task<Result> UpdateClassAsync(Classes ClassData, List<ServantWeeksDTO> servantWeeksDTOS, List<ServedWeeksDTO> servedWeeksDTOs)
        {
            // Retrieve the existing class and update its properties
            var existingClass = await _context.Classes
                .Include(c => c.Servants).ThenInclude(x => x.ServantWeek).Include(x => x.Served).ThenInclude(x => x.ServedWeeks)
                .SingleOrDefaultAsync(c => c.Id == ClassData.Id);

            if (existingClass == null)
            {
                // Return an error if the class does not exist
                return Result.Fail("Class not found");
            }

            existingClass.Name = ClassData.Name;
            existingClass.Intercessor = ClassData.Intercessor;
            existingClass.TimeId = ClassData.TimeId;
            existingClass.ServiceId = ClassData.ServiceId;

            // Update the ServantWeek records for each servant
            var allWeeks = _context.Weeks.OrderByDescending(x => x.Id).Take(NumberOfWeeksAppearInMarkup);

            foreach (var servant in existingClass.Servants)
            {
                // Get the list of checked week ids for the current servant
                var servantWeeksDTO = servantWeeksDTOS.FirstOrDefault(x => x.ServantId == servant.Id);
                if (servantWeeksDTO == null)
                    continue; // Skip if the servant is not found in the DTOs

                // Loop through all weeks, including those not displayed on the form
                foreach (var week in allWeeks)
                {
                    // Check if the checkbox for this week was checked
                    var weekChecked = servantWeeksDTO.IsWeekSelected[week.Id];

                    // Check if the current servant already has a ServantWeek object for this week
                    var existingServantWeek = servant.ServantWeek.FirstOrDefault(x => x.WeekId == week.Id);

                    if (weekChecked && existingServantWeek == null)
                    {
                        // Checkbox was checked and there is no existing ServantWeek object, so create a new one
                        var newServantWeek = new ServantWeek { ServantId = servant.Id, WeekId = week.Id };
                        _context.ServantWeek.Add(newServantWeek);
                    }
                    else if (!weekChecked && existingServantWeek != null)
                    {
                        // Checkbox was unchecked and there is an existing ServantWeek object, so delete it
                        _context.ServantWeek.Remove(existingServantWeek);
                    }
                }
            }

            // Update the ServedWeek records for each Served
            foreach (var served in existingClass.Served)
            {
                // Get the list of checked week ids for the current served
                var servedWeeksDTO = servedWeeksDTOs.FirstOrDefault(x => x.ServedId == served.Id);
                if (servedWeeksDTO == null)
                    continue; // Skip if the served is not found in the DTOs

                // Loop through all weeks, including those not displayed on the form
                foreach (var week in allWeeks)
                {
                    // Check if the checkbox for this week was checked
                    var weekChecked = servedWeeksDTO.IsWeekSelected[week.Id];

                    // Check if the current served already has a ServedWeek object for this week
                    var existingServedWeek = served.ServedWeeks.FirstOrDefault(x => x.WeekId == week.Id);

                    if (weekChecked && existingServedWeek == null)
                    {
                        // Checkbox was checked and there is no existing ServedWeek object, so create a new one
                        var newServedWeek = new ServedWeeks { ServedId = served.Id, WeekId = week.Id };
                        _context.ServedWeeks.Add(newServedWeek);
                    }
                    else if (!weekChecked && existingServedWeek != null)
                    {
                        // Checkbox was unchecked and there is an existing ServedWeek object, so delete it
                        _context.ServedWeeks.Remove(existingServedWeek);
                    }
                }
            }

            // Save changes to the database asynchronously
            await _context.SaveChangesAsync();

            return Result.Ok("Class updated successfully");
        }



        public string GetFormattedWeekDate(DateTime week, string Time)
        {
            ServiceTime serviceTime;
            Time = Time.Replace(" ", "");
            if (!Enum.TryParse(Time, out serviceTime))
            {
                throw new ArgumentException($"Invalid value for 'time': {Time}");
            }
            string FormatedDate = (serviceTime) switch
            {
                ServiceTime.الخميس => week.ToString("dd/MM/yyyy"),
                ServiceTime.الجمعةصباحا => week.AddDays(1).ToString("dd/MM/yyyy"),
                ServiceTime.الجمعةمساء => week.AddDays(1).ToString("dd/MM/yyyy"),
                ServiceTime.السبت => week.AddDays(2).ToString("dd/MM/yyyy"),
                _ => week.ToString("dd/MM/yyyy"),

            };
            return FormatedDate;
        }

        public List<ReminderEmailModel> GetServedNeedToBeRemembered()
        {
            List<ReminderEmailModel> reminderEmailModels = new List<ReminderEmailModel>();

            // Get Classes with Servants and Served relationships
            var classesWithServantsAndServed = _context.Classes
                .Where(c => c.Servants.Any() && c.Served.Any())
                .Include(c => c.Servants)
                .Include(c => c.Served)
                .ThenInclude(s => s.ServedWeeks)
                .ThenInclude(sw => sw.Week)
                .AsNoTracking();

            foreach (var sundaySchoolClass in classesWithServantsAndServed)
            {
                // Get Servants who receive reminder emails for the class
                List<Servants> servants = sundaySchoolClass.Servants
                    .Where(s => !string.IsNullOrEmpty(s.Email))
                    .ToList();

                // If no Servants are found, add the first Servant as fallback
                if (!servants.Any())
                {
                    servants.Add(sundaySchoolClass.Servants.FirstOrDefault());
                }

                var lastMonth = DateTime.Now.AddMonths(-1);

                // Get weeks from the last month
                var lastMonthWeeks = _context.Weeks
                    .Where(w => w.Date.Month == lastMonth.Month)
                    .AsNoTracking();

                List<Served> servedNeedToBeRemembered = new List<Served>();

                foreach (var served in sundaySchoolClass.Served)
                {
                    int missedWeeksCount = 0;

                    // Count missed weeks for the served person
                    foreach (var week in lastMonthWeeks)
                    {
                        if (!served.ServedWeeks.Any(sw => sw.WeekId == week.Id))
                        {
                            missedWeeksCount++;
                        }
                    }

                    if (missedWeeksCount >= 2)
                    {
                        servedNeedToBeRemembered.Add(served);
                    }
                }

                if (servedNeedToBeRemembered.Any())
                {
                    reminderEmailModels.Add(new ReminderEmailModel(servants, servedNeedToBeRemembered));
                }
            }

            return reminderEmailModels;
        }



        public List<BithdayEmailModel> GetServedNeedToBeRememberedforBithday()
        {
            var today = DateTime.Now;
            List<BithdayEmailModel> reminderEmailModels = new List<BithdayEmailModel>();

            // Get Classes with Servants and Served relationships
            var classesWithServantsAndServed = _context.Classes
                .Where(c => c.Servants.Any() && c.Served.Any())
                .Include(c => c.Servants)
                .Include(c => c.Served)
                .AsNoTracking();

            foreach (var sundaySchoolClass in classesWithServantsAndServed)
            {
                // Get Servants who receive reminder emails for the class
                List<Servants> servants = sundaySchoolClass.Servants
                    .Where(s => !string.IsNullOrEmpty(s.Email))
                    .ToList();

                // If no Servants are found, add the first Servant as fallback
                if (!servants.Any())
                {
                    servants.Add(sundaySchoolClass.Servants.FirstOrDefault());
                }



                List<Served> servedNeedToBeRemembered = sundaySchoolClass.Served
                    .Where(x => x.Birthday?.Month == today.Month && x.Birthday?.Day == today.Day).ToList();



                if (servedNeedToBeRemembered.Any())
                {
                    reminderEmailModels.Add(new BithdayEmailModel(servants, servedNeedToBeRemembered));
                }
            }

            return reminderEmailModels;

        }



    }
}
