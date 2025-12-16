using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Managers;

namespace App.Core.Infrastrcuture
{
    public class ExcelProcessor
    {
        private readonly ClassManager _classManager;
        private readonly WeekManager _weekManager;
        private int NumberOfWeeksAppearInExcelFile = 12;
        public ExcelProcessor(ClassManager classManager, WeekManager weekManager)
        {
            _classManager = classManager;
            _weekManager = weekManager;
        }
        public (string, byte[]) GenerateExcelAttendance(int ClassId)
        {
            var CurrentClass = _classManager.GetClass(ClassId);
            var ServantList = CurrentClass.Servants.ToList();
            var ServedList = CurrentClass.Served.ToList();
            var Weeks = _weekManager.GetWeeks(NumberOfWeeksAppearInExcelFile);
            byte[] result;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(string.Concat(CurrentClass.Name, " ", CurrentClass.Time.Time1));

                // Set Right-to-Left for Arabic
                worksheet.View.RightToLeft = true;

                int row = 1;

                // Title Row
                worksheet.Cells[row, 1].Value = $"كشف حضور وغياب - {CurrentClass.Name} - {CurrentClass.Time.Time1}";
                worksheet.Cells[row, 1, row, Weeks.Count() + 1].Merge = true;
                worksheet.Cells[row, 1].Style.Font.Size = 16;
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[row, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[row, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(42, 82, 152)); // Blue color
                worksheet.Cells[row, 1].Style.Font.Color.SetColor(Color.White);
                row++;

                // Header row with dates
                worksheet.Cells[row, 1].Value = "الاسم";
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                worksheet.Cells[row, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[row, 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                for (int i = 0; i < Weeks.Count(); i++)
                {
                    worksheet.Cells[row, (i + 2)].Value = _classManager.GetFormattedWeekDate(Weeks[i].Date, CurrentClass.Time.Time1);
                    worksheet.Cells[row, (i + 2)].Style.Font.Bold = true;
                    worksheet.Cells[row, (i + 2)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, (i + 2)].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    worksheet.Cells[row, (i + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                row++;

                // Servants Section
                worksheet.Cells[row, 1].Value = "الخدام";
                worksheet.Cells[row, 1, row, Weeks.Count() + 1].Merge = true;
                worksheet.Cells[row, 1].Style.Font.Size = 14;
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                worksheet.Cells[row, 1].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[row, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[row, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(220, 53, 69)); // Red color
                worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                row++;

                for (int i = 0; i < ServantList.Count(); i++)
                {
                    worksheet.Cells[row, 1].Value = ServantList[i].Name;
                    worksheet.Cells[row, 1].Style.Font.Bold = true;

                    for (int j = 0; j < Weeks.Count(); j++)
                    {
                        bool attended = ServantList[i].ServantWeek.Any(x => x.WeekId == Weeks[j].Id);
                        worksheet.Cells[row, (j + 2)].Value = attended ? "✓" : "✗";
                        worksheet.Cells[row, (j + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[row, (j + 2)].Style.Font.Size = 14;
                        worksheet.Cells[row, (j + 2)].Style.Font.Bold = true;

                        if (attended)
                        {
                            worksheet.Cells[row, (j + 2)].Style.Font.Color.SetColor(Color.Green);
                            worksheet.Cells[row, (j + 2)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[row, (j + 2)].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(212, 237, 218));
                        }
                        else
                        {
                            worksheet.Cells[row, (j + 2)].Style.Font.Color.SetColor(Color.Red);
                            worksheet.Cells[row, (j + 2)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[row, (j + 2)].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(248, 215, 218));
                        }
                    }
                    row++;
                }

                // Served Section
                row++;
                worksheet.Cells[row, 1].Value = "المخدومين";
                worksheet.Cells[row, 1, row, Weeks.Count() + 1].Merge = true;
                worksheet.Cells[row, 1].Style.Font.Size = 14;
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                worksheet.Cells[row, 1].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[row, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[row, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(220, 53, 69)); // Red color
                worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                row++;

                int servedStartRow = row;
                for (int i = 0; i < ServedList.Count(); i++)
                {
                    worksheet.Cells[row, 1].Value = ServedList[i].Name;

                    for (int j = 0; j < Weeks.Count(); j++)
                    {
                        bool attended = ServedList[i].ServedWeeks.Any(x => x.WeekId == Weeks[j].Id);
                        worksheet.Cells[row, (j + 2)].Value = attended ? "✓" : "✗";
                        worksheet.Cells[row, (j + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[row, (j + 2)].Style.Font.Size = 14;
                        worksheet.Cells[row, (j + 2)].Style.Font.Bold = true;

                        if (attended)
                        {
                            worksheet.Cells[row, (j + 2)].Style.Font.Color.SetColor(Color.Green);
                            worksheet.Cells[row, (j + 2)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[row, (j + 2)].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(212, 237, 218));
                        }
                        else
                        {
                            worksheet.Cells[row, (j + 2)].Style.Font.Color.SetColor(Color.Red);
                            worksheet.Cells[row, (j + 2)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[row, (j + 2)].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(248, 215, 218));
                        }
                    }
                    row++;
                }

                // Statistics Section
                row++;
                worksheet.Cells[row, 1].Value = "الإحصائيات";
                worksheet.Cells[row, 1, row, Weeks.Count() + 1].Merge = true;
                worksheet.Cells[row, 1].Style.Font.Size = 14;
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                worksheet.Cells[row, 1].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[row, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[row, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(42, 82, 152)); // Blue color
                worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                row++;

                // Total Served
                worksheet.Cells[row, 1].Value = "إجمالي المخدومين";
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                for (int j = 0; j < Weeks.Count(); j++)
                {
                    worksheet.Cells[row, (j + 2)].Value = ServedList.Count();
                    worksheet.Cells[row, (j + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[row, (j + 2)].Style.Font.Bold = true;
                }
                row++;

                // Attended Count
                worksheet.Cells[row, 1].Value = "عدد الحاضرين";
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                for (int j = 0; j < Weeks.Count(); j++)
                {
                    int attendedCount = ServedList.Count(s => s.ServedWeeks.Any(x => x.WeekId == Weeks[j].Id));
                    worksheet.Cells[row, (j + 2)].Value = attendedCount;
                    worksheet.Cells[row, (j + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[row, (j + 2)].Style.Font.Bold = true;
                    worksheet.Cells[row, (j + 2)].Style.Font.Color.SetColor(Color.Green);
                }
                row++;

                // Absent Count
                worksheet.Cells[row, 1].Value = "عدد الغائبين";
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                for (int j = 0; j < Weeks.Count(); j++)
                {
                    int attendedCount = ServedList.Count(s => s.ServedWeeks.Any(x => x.WeekId == Weeks[j].Id));
                    int absentCount = ServedList.Count() - attendedCount;
                    worksheet.Cells[row, (j + 2)].Value = absentCount;
                    worksheet.Cells[row, (j + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[row, (j + 2)].Style.Font.Bold = true;
                    worksheet.Cells[row, (j + 2)].Style.Font.Color.SetColor(Color.Red);
                }
                row++;

                // Attendance Percentage
                worksheet.Cells[row, 1].Value = "نسبة الحضور";
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                for (int j = 0; j < Weeks.Count(); j++)
                {
                    int attendedCount = ServedList.Count(s => s.ServedWeeks.Any(x => x.WeekId == Weeks[j].Id));
                    double percentage = ServedList.Count() > 0 ? (double)attendedCount / ServedList.Count() * 100 : 0;
                    worksheet.Cells[row, (j + 2)].Value = $"{percentage:F1}%";
                    worksheet.Cells[row, (j + 2)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[row, (j + 2)].Style.Font.Bold = true;

                    // Color based on percentage
                    if (percentage >= 80)
                        worksheet.Cells[row, (j + 2)].Style.Font.Color.SetColor(Color.Green);
                    else if (percentage >= 50)
                        worksheet.Cells[row, (j + 2)].Style.Font.Color.SetColor(Color.Orange);
                    else
                        worksheet.Cells[row, (j + 2)].Style.Font.Color.SetColor(Color.Red);
                }

                // Add borders to all cells
                var allCells = worksheet.Cells[1, 1, row, Weeks.Count() + 1];
                allCells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                allCells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                allCells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                allCells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                worksheet.Cells.AutoFitColumns();
                result = package.GetAsByteArray();
            }

            return ("كشف حضور وغياب" + " " + CurrentClass.Name + "_" + CurrentClass.Time.Time1 + ".xlsx", result);
        }

        public (string, byte[]) GenerateExcelClassDetails(int ClassId)
        {
            var CurrentClass = _classManager.GetClass(ClassId);
            var ServantList = CurrentClass.Servants.ToList();
            var ServedList = CurrentClass.Served.ToList();
            byte[] result;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add($"{CurrentClass.Name} {CurrentClass.Time.Time1}");

                // اتجاه الجدول من اليمين لليسار
                worksheet.View.RightToLeft = true;

                // الصف الأول - رؤوس الأعمدة
                int headerRow = 1;
                int col = 1;

                worksheet.Cells[headerRow, col++].Value = "الاسم";
                worksheet.Cells[headerRow, col++].Value = "الكود";
                worksheet.Cells[headerRow, col++].Value = "رقم التليفون";
                worksheet.Cells[headerRow, col++].Value = "العنوان";
                worksheet.Cells[headerRow, col++].Value = "اب الاعتراف";
                worksheet.Cells[headerRow, col++].Value = "تاريخ الميلاد";
                worksheet.Cells[headerRow, col++].Value = "تليفون المنزل";
                worksheet.Cells[headerRow, col++].Value = "الخادم المسئول";

                for (int i = 1; i <= col; i++)
                {
                    worksheet.Cells[headerRow, i].Style.Font.Size = 14;
                    worksheet.Cells[headerRow, i].Style.Font.Bold = true;
                }

                // الخدام
                int row = 2;
                worksheet.Cells[row, 1].Value = "الخدام";
                worksheet.Cells[row, 1, row, col].Merge = true;
                worksheet.Cells[row, 1].Style.Font.Size = 14;
                worksheet.Cells[row, 1].Style.Font.Color.SetColor(Color.Red);
                worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                row++;

                foreach (var servant in ServantList)
                {
                    col = 1;
                    worksheet.Cells[row, col++].Value = servant.Name;
                    worksheet.Cells[row, col++].Value = servant.Id;  // الكود
                    worksheet.Cells[row, col++].Value = servant.Phone;
                    worksheet.Cells[row, col++].Value = servant.Address;
                    worksheet.Cells[row, col++].Value = servant.FatherOfConfession;
                    worksheet.Cells[row, col++].Value = ""; // تاريخ الميلاد: لا ينطبق على الخدام
                    worksheet.Cells[row, col++].Value = ""; // تليفون المنزل: لا ينطبق على الخدام
                    worksheet.Cells[row, col++].Value = ""; // الخادم المسئول: لا ينطبق على الخدام
                    row++;
                }

                // المخدومين
                worksheet.Cells[row, 1].Value = "المخدومين";
                worksheet.Cells[row, 1, row, col].Merge = true;
                worksheet.Cells[row, 1].Style.Font.Size = 14;
                worksheet.Cells[row, 1].Style.Font.Color.SetColor(Color.Red);
                worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                row++;

                foreach (var served in ServedList)
                {
                    col = 1;
                    worksheet.Cells[row, col++].Value = served.Name;
                    worksheet.Cells[row, col++].Value = served.Id; // الكود
                    worksheet.Cells[row, col++].Value = served.Phone;
                    worksheet.Cells[row, col++].Value = served.Address;
                    worksheet.Cells[row, col++].Value = served.FatherOfConfession;
                    worksheet.Cells[row, col++].Value = served.Birthday?.ToString("dd / MM / yyyy") ?? "";
                    worksheet.Cells[row, col++].Value = served.HomePhone ?? ""; // تليفون المنزل
                    worksheet.Cells[row, col++].Value = served.ResponsibleServant;
                    row++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                result = package.GetAsByteArray();
            }

            return ($"كشف بيانات الفصل {CurrentClass.Name}_{CurrentClass.Time.Time1}.xlsx", result);
        }

    }
}
