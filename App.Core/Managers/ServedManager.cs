using App.Core.Entities;
using App.Core.Models;
using Microsoft.EntityFrameworkCore;
using AppCore.Common;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;

namespace App.Core.Managers
{
    public class ServedManager
    {
        private readonly MarminaAttendanceContext _context;
        private int NumberOfWeeksAppearInMarkup = 16;

        public ServedManager(MarminaAttendanceContext context)
        {
            _context = context;
        }
        public Result AddServed(Served newServed)
        {
            try
            {
                _context.Served.Add(newServed);
                _context.SaveChanges();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.InnerException?.Message);
            }

        }

        public List<ServedVM> GetServeds()
        {
            var Serveds = _context.Served.Include(s => s.Class).Include(x=>x.ResponsibleServant).AsNoTracking().Select(x => new ServedVM
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                Birthday = x.Birthday.HasValue ? x.Birthday.Value.ToString("dd/MM/yyyy") : string.Empty,
                ClassName = x.Class.Name,
                ResponsibleServant=x.ResponsibleServant.Name,
                FatherOfConfession = x.FatherOfConfession,
                Phone = x.Phone,
                Photo = x.Photo,
            }).ToList();
            return Serveds;
        }
        public Result DeleteServed(int ServedId)
        {
            try
            {
                var existServed = _context.Served.Find(ServedId);
                if(existServed == null)
                {
                    return Result.Fail("تم مسح المخدوم بالفعل");
                }
                _context.Served.Remove(existServed);
                _context.SaveChanges();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.InnerException.Message);
            }
        }
        public Result UpdateServed(Served Served, ServedWeeksDTO ServedWeeksDTO)
        {
            var existServed = _context.Served.Where(x => x.Id == Served.Id).Include(x => x.ServedWeeks).FirstOrDefault();


            if (existServed == null)
            {
                // Return an error if the class does not exist
                return Result.Fail("الخادم غير موجود");
            }
            existServed.Address = Served.Address;
            existServed.Birthday = Served.Birthday;
            existServed.Phone = Served.Phone;
            existServed.ResponsibleServantId= Served.ResponsibleServantId;
            if (!string.IsNullOrEmpty(Served.Photo))
            {
                existServed.Photo = Served.Photo;
            }
            existServed.Name = Served.Name;
            existServed.ClassId = Served.ClassId;
            existServed.FatherOfConfession = Served.FatherOfConfession;
            existServed.Notes = Served.Notes;
            var allWeeks = _context.Weeks.OrderByDescending(x => x.Id).Take(NumberOfWeeksAppearInMarkup);
            foreach (var week in allWeeks)
            {
                var weekChecked = ServedWeeksDTO.IsWeekSelected[week.Id];
                var existingServedWeek = existServed.ServedWeeks.FirstOrDefault(x => x.WeekId == week.Id);
                if (weekChecked && existingServedWeek == null)
                {
                    // Checkbox was checked and there is no existing ServedWeek object, so create a new one
                    var newServedWeek = new ServedWeeks { ServedId = Served.Id, WeekId = week.Id };
                    _context.ServedWeeks.Add(newServedWeek);
                }
                else if (!weekChecked && existingServedWeek != null)
                {
                    // Checkbox was unchecked and there is an existing ServedWeek object, so delete it
                    _context.ServedWeeks.Remove(existingServedWeek);
                }
                // Save changes to the database

            }
            _context.SaveChanges();
            return Result.Ok("Class updated successfully");
        }
        public Served GetServed(int ServedId)
        {
            var Served = _context.Served.Where(x => x.Id == ServedId)
                .Include(x => x.ServedWeeks).Include(x => x.Class).ThenInclude(x => x.Time).AsNoTracking().FirstOrDefault();
            return Served;
        }

        private ServedVM MapToServedVM(Served Served)
        {
            return new ServedVM
            {
                Id = Served.Id,
                Name = Served.Name,
                Photo = Served.Photo,
                ClassName = Served.Class.Name
            };
        }

        public Result<ServedVM> SearchServeds(string searchInput)
        {
            int ServedId;
            bool isNumeric = int.TryParse(searchInput, out ServedId);

            var Served = _context.Served
                .Where(x => (isNumeric && x.Id == ServedId) || (!isNumeric && x.Name.Trim().Contains(searchInput.Trim())))
                .Include(x => x.Class)
                .AsNoTracking()
                .FirstOrDefault();

            if (Served == null)
            {
                return Result.Fail<ServedVM>("Served not found");
            }

            return Result.Ok(MapToServedVM(Served));
        }
        public Result AttendanceRegistration(int ServedId)
        {
            var WeekId = _context.Weeks.AsNoTracking().OrderByDescending(w => w.Id).First().Id;
            var exists = _context.ServedWeeks.Any(x => x.ServedId == ServedId && x.WeekId == WeekId);
            if (exists)
            {
                return Result.Ok("تم تسجل حضور الخادم من قبل بالفعل");
            }
            try
            {
                _context.ServedWeeks.Add(new ServedWeeks { ServedId = ServedId, WeekId = WeekId });
                _context.SaveChanges();
                return Result.Ok("تم تسجيل الحضور");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
        public async Task<Result<List<Served>>> ServedBulkInsertAsync(IFormFile formFile, int classId)
        {
            var ImportExcelResult = await ImportExcelAsync(formFile, classId);
            if (ImportExcelResult.IsFailure)
            {
                return ImportExcelResult;
            }

            try
            {
                var servedList = ImportExcelResult.Value.ToList();
                //foreach (var servent in servedList)
                //{
                //    _context.Served.Add(servent);
                //}
                _context.AddRange(servedList);
                _context.SaveChanges();
                return Result.Ok(servedList);
            }
            catch (Exception ex)
            {
                return Result.Fail<List<Served>>(ex.Message);
            }
        }

        private async Task<Result<List<Served>>> ImportExcelAsync(IFormFile formFile, int classId)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if (formFile == null || formFile.Length <= 0)
            {
                return Result.Fail<List<Served>>("File Is Empty");
            }

            // check for valid file extension
            var fileExtension = Path.GetExtension(formFile.FileName);
            if (!fileExtension.Equals(".xlsx", StringComparison.OrdinalIgnoreCase) &&
                !fileExtension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
            {
                return Result.Fail<List<Served>>("Unsupported file extension. Please upload .xlsx or .csv files only.");
            }

            

            try
            {
                var records = new List<Served>();
                using (var reader = new StreamReader(formFile.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                  
                    csv.Read();
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        var record = new Served
                        {
                            Name = csv.GetField<string>("الاسم"),
                            Phone= csv.GetField<string>("رقم التليفون"),
                            Address=csv.GetField<string>("العنوان"),
                            FatherOfConfession=csv.GetField<string>("اب الاعتراف"),
                            ClassId=classId,
                            Birthday=csv.GetField<DateTime>("تاريخ الميلاد")

                        };
                        records.Add(record);
                    }
                }

                return Result.Ok(records);
            }
            catch (Exception ex)
            {
                return Result.Fail<List<Served>>(ex.Message);
            }

          
        }

    }
}
