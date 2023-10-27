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

                // Add header row
                worksheet.Cells[1, 1].Value = " ";

                // Add data rows
                int row = 1;
                for (int i = 0; i < Weeks.Count(); i++)
                {
                    worksheet.Cells[row, (i + 2)].Value =_classManager.GetFormattedWeekDate(Weeks[i].Date, CurrentClass.Time.Time1);

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
    }
}
