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

            if (currentServant != null)
            {
                // If servant has both ServiceId and ClassId, show all classes in service OR their own class
                if (currentServant.ServiceId.HasValue && currentServant.ClassId.HasValue)
                {
                    query = query.Where(c => c.ServiceId == currentServant.ServiceId || c.Id == currentServant.ClassId);
                }
                // If servant has only ServiceId, show all classes in service
                else if (currentServant.ServiceId.HasValue)
                {
                    query = query.Where(c => c.ServiceId == currentServant.ServiceId);
                }
                // If servant has only ClassId, show only their class
                else if (currentServant.ClassId.HasValue)
                {
                    query = query.Where(c => c.Id == currentServant.ClassId);
                }
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
            var reminderEmailModels = new List<ReminderEmailModel>();
            var lastMonth = DateTime.Now.AddMonths(-1);
            var lastMonthWeekIds = _context.Weeks
                .Where(w => w.Date.Month == lastMonth.Month)
                .Select(w => w.Id)
                .ToList();

            var classes = GetClassesWithServantsAndServed(includeServedWeeks: true);

            foreach (var sundaySchoolClass in classes)
            {
                var (classServants, serviceAdminEmails) = GetClassServantsAndAdminEmails(sundaySchoolClass);

                var absentServed = sundaySchoolClass.Served
                    .Where(served => CountMissedWeeks(served, lastMonthWeekIds) >= 2)
                    .ToList();

                if (absentServed.Any() && classServants.Any())
                {
                    reminderEmailModels.Add(new ReminderEmailModel(classServants, absentServed, serviceAdminEmails));
                }
            }

            return reminderEmailModels;
        }

        public List<BithdayEmailModel> GetServedNeedToBeRememberedforBithday()
        {
            var today = DateTime.Now;
            var birthdayEmailModels = new List<BithdayEmailModel>();

            var classes = GetClassesWithServantsAndServed(includeServedWeeks: false);

            foreach (var sundaySchoolClass in classes)
            {
                var (classServants, serviceAdminEmails) = GetClassServantsAndAdminEmails(sundaySchoolClass, addFallback: true);

                var birthdayServed = sundaySchoolClass.Served
                    .Where(s => s.Birthday?.Month == today.Month && s.Birthday?.Day == today.Day)
                    .ToList();

                if (birthdayServed.Any() && classServants.Any())
                {
                    birthdayEmailModels.Add(new BithdayEmailModel(classServants, birthdayServed, serviceAdminEmails));
                }
            }

            return birthdayEmailModels;
        }

        #region Private Helper Methods

        private IEnumerable<Classes> GetClassesWithServantsAndServed(bool includeServedWeeks)
        {
            IQueryable<Classes> query = _context.Classes
                .Where(c => c.Servants.Any() && c.Served.Any())
                .Include(c => c.Servants)
                .Include(c => c.Service);

            if (includeServedWeeks)
            {
                query = query
                    .Include(c => c.Served)
                    .ThenInclude(s => s.ServedWeeks);
            }
            else
            {
                query = query.Include(c => c.Served);
            }

            return query.AsNoTracking().ToList();
        }

        private (List<Servants> classServants, List<string> serviceAdminEmails) GetClassServantsAndAdminEmails(
            Classes sundaySchoolClass,
            bool addFallback = false)
        {
            var classServants = sundaySchoolClass.Servants
                .Where(s => !string.IsNullOrEmpty(s.Email) && s.ServiceId == null)
                .ToList();

            var serviceAdminEmails = sundaySchoolClass.Servants
                .Where(s => s.ServiceId != null && !string.IsNullOrEmpty(s.Email))
                .Select(s => s.Email)
                .ToList();

            if (addFallback && !classServants.Any())
            {
                var fallbackServant = sundaySchoolClass.Servants.FirstOrDefault();
                if (fallbackServant != null)
                {
                    classServants.Add(fallbackServant);
                }
            }

            return (classServants, serviceAdminEmails);
        }

        private int CountMissedWeeks(Served served, List<int> weekIds)
        {
            return weekIds.Count(weekId => !served.ServedWeeks.Any(sw => sw.WeekId == weekId));
        }

        #endregion



    }
}
