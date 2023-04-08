using App.Core.Entities;
using App.Core.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Core.Common;

namespace App.Core.Managers
{
    public class ServantManager
    {
        private readonly MarminaAttendanceContext _context;
        private int NumberOfWeeksAppearInMarkup = 16;

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

        public List<ServantVM> GetServants()
        {
            var Servants = _context.Servants.Include(s => s.Class).AsNoTracking().Select(x => new ServantVM
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                Birthday = x.Birthday.Value.ToShortDateString(),
                ClassName = x.Class.Name,
                FatherOfConfession = x.FatherOfConfession,
                Phone = x.Phone,
                Photo = x.Photo,
            }).ToList();
            return Servants;
        }
        public Result DeleteServant(int servantId)
        {
            try
            {
                var existServant = _context.Servants.Find(servantId);

                _context.Servants.Remove(existServant);
                _context.SaveChanges();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.InnerException.Message);
            }
        }
        public Result UpdateServant(Servants servant,ServantWeeksDTO servantWeeksDTO)
        {
            var existServant = _context.Servants.Where(x => x.Id == servant.Id).Include(x => x.ServantWeek).FirstOrDefault();


            if (existServant == null)
            {
                // Return an error if the class does not exist
                return Result.Fail("الخادم غير موجود");
            }
            existServant.Address = servant.Address;
            existServant.Birthday = servant.Birthday;
            existServant.Phone = servant.Phone;
            if (!string.IsNullOrEmpty(servant.Photo))
            {
                existServant.Photo = servant.Photo;
            }
            existServant.Name = servant.Name;
            existServant.ClassId = servant.ClassId;
            existServant.FatherOfConfession = servant.FatherOfConfession;
            existServant.Leader = servant.Leader;
            var allWeeks = _context.Weeks.OrderByDescending(x => x.Id).Take(NumberOfWeeksAppearInMarkup);
            foreach (var week in allWeeks)
            {
                var weekChecked = servantWeeksDTO.IsWeekSelected[week.Id];
                var existingServantWeek = existServant.ServantWeek.FirstOrDefault(x => x.WeekId == week.Id);
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
                // Save changes to the database

            }
            _context.SaveChanges();
            return Result.Ok("Class updated successfully");
        }
        public Servants GetServant(int servantId)
        {
            var servant = _context.Servants.Where(x => x.Id == servantId)
                .Include(x => x.ServantWeek).Include(x=>x.Class).ThenInclude(x=>x.Time).AsNoTracking().FirstOrDefault();
            return servant;
        }
    }
}
