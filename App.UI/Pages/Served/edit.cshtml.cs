using App.Core.Entities;
using App.Core.Managers;
using App.Core.Models;
using App.UI.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.UI.Pages.Serveds
{
    [Authorize]
    public class EditcshtmlModel(ServedManager ServedManager, WeekManager weekManager, ClassManager classManager) : PageModel
    {
        private readonly ServedManager ServedManager = ServedManager;
        private readonly WeekManager weekManager = weekManager;
        private readonly ClassManager classManager = classManager;
        private readonly int NumberOfWeeksAppearInMarkup = 16;
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public List<Weeks> WeeksList { get; private set; }
        [BindProperty]
        public Served Served { get; set; }
        [BindProperty]
        public ServedWeeksDTO ServedWeeksDTO { get; set; }
        public SelectList Classes { get; private set; }

        public void OnGet()
        {
            FillData();
        }
        public void FillData()
        {

            WeeksList = weekManager.GetWeeks(NumberOfWeeksAppearInMarkup);
            Served = ServedManager.GetServed(Id);
            Classes = new SelectList(classManager.GetClasses(), "Id", "Name");

            // Initialize ServedWeeksDTO with all weeks
            ServedWeeksDTO = new ServedWeeksDTO();
            foreach (var week in WeeksList)
            {
                ServedWeeksDTO.IsWeekSelected[week.Id] = false;
            }
        }
        public void OnPost()
        {
            if (Served.PhotoFile != null)
            {
                Served.Photo = FileManager.UploadPhoto(Served.PhotoFile, "wwwroot/photos/Served/", 285, 310);

            }
            var Result = ServedManager.UpdateServed(Served, ServedWeeksDTO);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم تحديث البيانات بنجاح" : Result.Error;
            FillData();

        }
        public string GetFormattedWeekDate(DateTime week, string Time)
        {
            return classManager.GetFormattedWeekDate(week, Time);
        }
    }
}
