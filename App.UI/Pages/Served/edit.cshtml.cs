using App.Core.Entities;
using App.Core.Enums;
using App.Core.Managers;
using App.Core.Models;
using App.UI.Ifraustrcuture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.UI.Pages.Serveds
{
    public class EditcshtmlModel : PageModel
    {
        private readonly ServedManager ServedManager;
        private readonly WeekManager weekManager;
        private readonly ClassManager classManager;
        private readonly int NumberOfWeeksAppearInMarkup = 16;
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public List<Weeks> WeeksList { get; private set; }
        [BindProperty]
        public Served Served { get; set; }
        [BindProperty]
        public ServedWeeksDTO ServedWeeksDTO { get; set; }
        public SelectList Classes { get; private set; }

        public EditcshtmlModel(ServedManager ServedManager, WeekManager weekManager, ClassManager classManager)
        {
            this.ServedManager = ServedManager;
            this.weekManager = weekManager;
            this.classManager = classManager;
        }
        public void OnGet()
        {
            FillData();
        }
        public void FillData()
        {

            WeeksList = weekManager.GetWeeks(NumberOfWeeksAppearInMarkup);
            Served = ServedManager.GetServed(Id);
            Classes = new SelectList(classManager.GetClasses(), "Id", "Name");
        }
        public void OnPost()
        {
            if (Served.PhotoFile != null)
            {
                Served.Photo = FileManager.UploadPhoto(Served.PhotoFile, "/wwwroot/photos/المخدومين/", 285, 310);

            }
            var Result = ServedManager.UpdateServed(Served, ServedWeeksDTO);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم تحديث البيانات بنجاح" : Result.Error;
            FillData();

        }
        public string GetFormattedWeekDate(DateTime week, string Time)
        {
            ServiceTime serviceTime;
            if (!Enum.TryParse(Time.Replace(" ", ""), out serviceTime))
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
    }
}
