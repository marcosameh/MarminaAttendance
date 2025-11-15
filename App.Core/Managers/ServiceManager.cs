using App.Core.Entities;
using App.Core.Models;
using AppCore.Common;
using Microsoft.EntityFrameworkCore;

namespace App.Core.Managers
{
    public class ServiceManager
    {
        private readonly MarminaAttendanceContext _context;

        public ServiceManager(MarminaAttendanceContext context)
        {
            _context = context;
        }

        public List<ServiceVM> GetServices()
        {
            return _context.Services
                .OrderBy(x => x.Name)
                .Select(x => new ServiceVM
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
        }

        public List<Services> GetServicesList()
        {
            return _context.Services
                .OrderBy(x => x.Name)
                .ToList();
        }

        public Result AddService(Services serviceData)
        {
            try
            {
                _context.Services.Add(serviceData);
                _context.SaveChanges();
                return Result.Ok("تم اضافة الخدمة بنجاح");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public Result DeleteService(int id)
        {
            try
            {
                var existService = _context.Services.Find(id);
                if (existService == null)
                {
                    return Result.Fail("تم مسح الخدمة بالفعل");
                }
                _context.Services.Remove(existService);
                _context.SaveChanges();
                return Result.Ok("تم مسح الخدمة بنجاح");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public Services GetService(int id)
        {
            return _context.Services
                .Where(x => x.Id == id)
                .AsNoTracking()
                .FirstOrDefault();
        }

        public Result UpdateService(Services serviceData)
        {
            try
            {
                var existingService = _context.Services.Find(serviceData.Id);
                if (existingService == null)
                {
                    return Result.Fail("الخدمة غير موجودة");
                }

                existingService.Name = serviceData.Name;
                _context.SaveChanges();
                return Result.Ok("تم تحديث الخدمة بنجاح");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
