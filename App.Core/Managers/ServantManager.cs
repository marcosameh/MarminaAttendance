using App.Core.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Core.Common;

namespace App.Core.Managers
{
    public class ServantManager
    {
        private readonly MarminaAttendanceContext _context;
        private int NumberOfWeeksAppearInMarkup=5;

        public ServantManager(MarminaAttendanceContext context)
        {
            _context = context;
        }
        public Result AddServant(Servants newServant)
        {
            try
            {
                _context.Servants.Add(newServant);
                _context.SaveChanges();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }

        }
        public Result UpdateClass(Classes ClassData, Dictionary<int, List<int>> checkedServantWeeks)
        {
            // Retrieve the existing class and update its properties
            var existingClass = _context.Classes
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
            var allWeeks = _context.Weeks.OrderByDescending(x => x.Id).Take(NumberOfWeeksAppearInMarkup);
            foreach (var servant in existingClass.Servants)
            {
                // Get the list of checked week ids for the current servant
                var checkedWeekIds = checkedServantWeeks.GetValueOrDefault(servant.Id, new List<int>());
                // Loop through all weeks, including those not displayed on the form
                foreach (var week in allWeeks)
                {
                    // Check if the checkbox for this week was checked
                    var weekChecked = checkedWeekIds.Contains(week.Id);

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


            // Save changes to the database
            _context.SaveChanges();
            return Result.Ok("Class updated successfully");


        }


    }
}
