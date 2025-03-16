﻿using App.Core.Entities;
using App.Core.Models;
using AppCore.Common;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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
                return Result.Fail($"{ex.Message}{(ex.InnerException != null ? $" Inner Exception: {ex.InnerException.Message}" : "")}");
            }


        }

        public List<ServantVM> GetServants(int? classId)
        {

            var Servants = _context.Servants
                .Where(x => !classId.HasValue || x.ClassId == classId)
                .Include(s => s.Class)
                .OrderBy(x => x.Name)
                .AsNoTracking()
                .Select(x => new ServantVM
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    Email = x.Email,
                    ClassName = x.Class.Name,
                    FatherOfConfession = x.FatherOfConfession,
                    Phone = x.Phone,
                    Photo = x.Photo,
                }).ToList();
            return Servants;
        }
        public List<ServantVM> GetServants()
        {

            var Servants = _context.Servants
                .Include(s => s.Class)
                .OrderBy(x => x.Name)
                .AsNoTracking()
                .Select(x => new ServantVM
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    Email = x.Email,
                    ClassName = x.Class.Name,
                    FatherOfConfession = x.FatherOfConfession,
                    Phone = x.Phone,
                    Photo = x.Photo,
                }).ToList();
            return Servants;
        }

        public List<ServantVM> GetResponsibleServants(int classId)
        {
            var Servants = _context.Servants.Where(s => s.ClassId == classId).AsNoTracking().OrderBy(x => x.Name).Select(x => new ServantVM
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                Email = x.Email,
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
                if (existServant == null)
                {
                    return Result.Fail("تم مسح الخادم بالفعل");
                }
                _context.Servants.Remove(existServant);
                _context.SaveChanges();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail($"{ex.Message}{(ex.InnerException != null ? $" Inner Exception: {ex.InnerException.Message}" : "")}");
            }

        }
        public Result UpdateServant(Servants servant, ServantWeeksDTO servantWeeksDTO)
        {
            var existServant = _context.Servants.Where(x => x.Id == servant.Id).Include(x => x.ServantWeek).FirstOrDefault();


            if (existServant == null)
            {
                // Return an error if the class does not exist
                return Result.Fail("الخادم غير موجود");
            }
            existServant.Address = servant.Address;
            existServant.Email = servant.Email;
            existServant.Phone = servant.Phone;
            if (!string.IsNullOrEmpty(servant.Photo))
            {
                existServant.Photo = servant.Photo;
            }
            existServant.Name = servant.Name;
            existServant.ClassId = servant.ClassId;
            existServant.FatherOfConfession = servant.FatherOfConfession;
            existServant.Leader = servant.Leader;
            existServant.ReceiveReminderEmails = servant.ReceiveReminderEmails;
            existServant.Notes = servant.Notes;
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
                .Include(x => x.ServantWeek).Include(x => x.Class).ThenInclude(x => x.Time).AsNoTracking().FirstOrDefault();
            return servant;
        }

        private ServantVM MapToServantVM(Servants servant)
        {
            return new ServantVM
            {
                Id = servant.Id,
                Name = servant.Name,
                Photo = servant.Photo,
                ClassName = servant.Class.Name
            };
        }

        public Result<ServantVM> SearchServants(string searchInput)
        {
            int servantId;
            bool isNumeric = int.TryParse(searchInput, out servantId);

            var servant = _context.Servants
                .Where(x => (isNumeric && x.Id == servantId) || (!isNumeric && x.Name.Trim().Contains(searchInput.Trim())))
                .Include(x => x.Class)
                .AsNoTracking()
                .FirstOrDefault();

            if (servant == null)
            {
                return Result.Fail<ServantVM>("Servant not found");
            }

            return Result.Ok(MapToServantVM(servant));
        }
        
        public Result<string> AttendanceRegistration(int servantId)
        {
            var Week = _context.Weeks.AsNoTracking().OrderByDescending(w => w.Id).First();
            var exists = _context.ServantWeek.Any(x => x.ServantId == servantId && x.WeekId == Week.Id);
            var servant = _context.Servants.AsNoTracking()
                .Include(s => s.Class)
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == servantId);

            var today = DateTime.Now.Date;
            if (exists)
            {
                return Result.Fail<string>($"تم تسجل حضور الخادم {servant.Name} من قبل بالفعل");
            }
            var daysToAdd = DaysToAdd(servant.Class.TimeId);
            if (today != Week.Date.AddDays(daysToAdd).Date)
            {
                return Result.Fail<string>("يوم الخدمة لم ياتى بعد");
            }
            try
            {
                _context.ServantWeek.Add(new ServantWeek { ServantId = servantId, WeekId = Week.Id });
                _context.SaveChanges();
                return Result.Ok($"{servant.Name} تم تسجيل الحضور");
            }
            catch (Exception ex)
            {
                return Result.Fail<string>(ex.Message);
            }
        }

        private int DaysToAdd(int timeId)
        {

            int DaysToAdd = (timeId) switch
            {
                /*ServiceTime.الخميس*/
                1 => 0,
                /* ServiceTime.الجمعةصباحا*/
                2 => 1,
                /* ServiceTime.الجمعةمساء*/
                3 => 1,
                /*ServiceTime.السبت*/
                4 => 2,
                _ => 0,

            };
            return DaysToAdd;
        }

    }
}
