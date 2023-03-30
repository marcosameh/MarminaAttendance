using App.Core.Entities;
using App.Core.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Core.Common;

namespace App.Core.Managers
{
    public class ServantManager
    {
        private readonly MarminaAttendanceContext _context;
        private int NumberOfWeeksAppearInMarkup = 5;

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

        public Servants GetServant(int servantId)
        {
            var servant = _context.Servants.Where(x => x.Id == servantId)
                .Include(x => x.ServantWeek).FirstOrDefault();
            return servant;
        }
    }
}
