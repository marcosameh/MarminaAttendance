using App.Core.Entities;
using App.Core.Models;
using Microsoft.EntityFrameworkCore;
using AppCore.Common;
using System.Runtime.InteropServices;
using OfficeOpenXml;
using App.Core.Enums;
using AppCore.Utilities;
using OfficeOpenXml.Style;
using System.Drawing;
using Microsoft.AspNetCore.Http;

namespace App.Core.Managers
{
    public class ClassManager
    {
        private readonly MarminaAttendanceContext _context;
        private int NumberOfWeeksAppearInMarkup = 5;
        private int NumberOfWeeksAppearInExcelFile = 12;
        public ClassManager(MarminaAttendanceContext context)
        {
            _context = context;
        }
        public List<ClassVM> GetClasses()
        {

            return _context.Classes.Include(x => x.Time).OrderBy(x=>x.Name).Select(x => new ClassVM
            {
                Id = x.Id,
                Name = x.Name,
                Intercessor = x.Intercessor,
                Time = x.Time.Time1
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
            return _context.Classes.Where(x => x.Id == id).Include(x => x.Servants).ThenInclude(x => x.ServantWeek).
                Include(x => x.Served).ThenInclude(x => x.ServedWeeks).Include(x => x.Time).AsSplitQuery().AsNoTracking().FirstOrDefault();

        }
        public Result UpdateClass(Classes ClassData, List<ServantWeeksDTO> servantWeeksDTOS, List<ServedWeeksDTO> servedWeeksDTOs)
        {
            // Retrieve the existing class and update its properties
            var existingClass = _context.Classes
                .Include(c => c.Servants).ThenInclude(x => x.ServantWeek).Include(x => x.Served).ThenInclude(x => x.ServedWeeks)
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
                var servantWeeksDTO = servantWeeksDTOS.Where(x => x.ServantId == servant.Id).FirstOrDefault();
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
                // Get the list of checked week ids for the current servant
                var servedWeeksDTO = servedWeeksDTOs.Where(x => x.ServedId == served.Id).FirstOrDefault();
                // Loop through all weeks, including those not displayed on the form
                foreach (var week in allWeeks)
                {
                    // Check if the checkbox for this week was checked
                    var weekChecked = servedWeeksDTO.IsWeekSelected[week.Id];

                    // Check if the current servant already has a ServantWeek object for this week
                    var existingServedWeek = served.ServedWeeks.FirstOrDefault(x => x.WeekId == week.Id);

                    if (weekChecked && existingServedWeek == null)
                    {
                        // Checkbox was checked and there is no existing ServantWeek object, so create a new one
                        var newServedWeek = new ServedWeeks { ServedId = served.Id, WeekId = week.Id };
                        _context.ServedWeeks.Add(newServedWeek);
                    }
                    else if (!weekChecked && existingServedWeek != null)
                    {
                        // Checkbox was unchecked and there is an existing ServantWeek object, so delete it
                        _context.ServedWeeks.Remove(existingServedWeek);
                    }
                }

            }



            // Save changes to the database
            _context.SaveChanges();
            return Result.Ok("Class updated successfully");


        }

        public (string, byte[]) GenerateExcelFile(int ClassId)
        {
            var CurrentClass = GetClass(ClassId);
            var ServantList = CurrentClass.Servants.ToList();
            var ServedList = CurrentClass.Served.ToList();
            var Weeks = _context.Weeks.OrderByDescending(x => x.Id).Take(NumberOfWeeksAppearInExcelFile).AsNoTracking().ToList();
            byte[] result;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(string.Concat(CurrentClass.Name, " ", CurrentClass.Time.Time1));

                // Add header row
                worksheet.Cells[1, 1].Value = " ";

                // Add data rows
                int row = 1;
                for (int i = 0; i < Weeks.Count(); i++)
                {
                    worksheet.Cells[row, (i + 2)].Value = GetFormattedWeekDate(Weeks[i].Date, CurrentClass.Time.Time1);

                }

                row = 2;
                worksheet.Cells[2, 1].Value = "الخدام";
                worksheet.Cells[2, 1].Style.Font.Size = 14;
                worksheet.Cells[2, 1].Style.Font.Color.SetColor(Color.Red);
                row = 3;
                for (int i = 0; i < ServantList.Count(); i++)
                {

                    worksheet.Cells[row, 1].Value = ServantList[i].Name;
                    for (int j = 0; j < Weeks.Count(); j++)
                    {
                        if (ServantList[i].ServantWeek.Where(x => x.WeekId == Weeks[j].Id).Any())
                        {
                            worksheet.Cells[row, (j + 2)].Value = "+";
                            worksheet.Cells[row, (j + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[row, (j + 2)].Style.Font.Size = 14;
                        }
                        else
                        {
                            worksheet.Cells[row, (j + 2)].Value = "-";
                            worksheet.Cells[row, (j + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[row, (j + 2)].Style.Font.Size = 14;
                        }
                    }
                    row++;
                }
                worksheet.Cells[row, 1].Value = "المخدومين";
                worksheet.Cells[row, 1].Style.Font.Size = 14;
                worksheet.Cells[row, 1].Style.Font.Color.SetColor(Color.Red);
                row++;
                for (int i = 0; i < ServedList.Count(); i++)
                {

                    worksheet.Cells[row, 1].Value = ServedList[i].Name;
                    for (int j = 0; j < Weeks.Count(); j++)
                    {
                        if (ServedList[i].ServedWeeks.Where(x => x.WeekId == Weeks[j].Id).Any())
                        {
                            worksheet.Cells[row, (j + 2)].Value = "+";
                            worksheet.Cells[row, (j + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[row, (j + 2)].Style.Font.Size = 14;
                        }
                        else
                        {
                            worksheet.Cells[row, (j + 2)].Value = "-";
                            worksheet.Cells[row, (j + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[row, (j + 2)].Style.Font.Size = 14;
                        }
                    }
                    row++;
                }
                worksheet.Cells.AutoFitColumns();
                result = package.GetAsByteArray();
            }

            return (CurrentClass.Name + "_" + CurrentClass.Time.Time1 + ".xlsx", result);
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
                    .Where(s => s.ReceiveReminderEmails)
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

       



    }
}
