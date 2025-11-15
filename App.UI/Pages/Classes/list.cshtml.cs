using App.Core.Entities;
using App.Core.Infrastrcuture;
using App.Core.Managers;
using App.Core.Models;
using MarminaAttendance.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace App.UI.Pages.Classes
{
    [Authorize]

    public class listModel : PageModel
    {
        private readonly ClassManager classManager;
        private readonly TimeManager timeManager;
        private readonly ExcelProcessor excelProcessor;
        private readonly ServiceManager serviceManager;

        public IQueryable<ClassVM> Classes { get; set; }
        [BindProperty]
        public App.Core.Entities.Classes classData { get; set; }
        public List<Time> TimeList { get; private set; }
        public List<Services> ServiceList { get; private set; }

        public listModel(ClassManager classManager,
            TimeManager timeManager,
            ExcelProcessor excelProcessor,
            CustomUserManager CustomUserManager,
            ServiceManager serviceManager)
        {
            this.classManager = classManager;
            this.timeManager = timeManager;
            this.excelProcessor = excelProcessor;
            this.serviceManager = serviceManager;
        }
        public void OnGet()
        {
            FillData();
        }
        public async Task<IActionResult> OnGetDisplayClasses()
        {

            Classes = await classManager.GetFilteredClassesQueryAsync();

            return new JsonResult(Classes);
        }
        public void OnPost()
        {
            var Result = classManager.AddClass(classData);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم اضافة الفصل بنجاح" : Result.Error;
            FillData();

        }

        public void OnGetDelete(int id)
        {
            var Result = classManager.DeleteClass(id);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم مسح الفصل بنجاح" : Result.Error;
            FillData();
        }
        private void FillData()
        {
            TimeList = timeManager.GetTimeList();
            ServiceList = serviceManager.GetServicesList();
        }

        public IActionResult OnGetDownloadExcelAttendance(int classId)
        {
            FillData();
            (string fileName, byte[] excelData) = excelProcessor.GenerateExcelAttendance(classId);
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

        }
        public IActionResult OnGetDownloadExcelClassDetails(int classId)
        {
            FillData();
            (string fileName, byte[] excelData) = excelProcessor.GenerateExcelClassDetails(classId);
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

        }
    }
}
