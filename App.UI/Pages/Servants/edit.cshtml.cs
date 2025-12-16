using App.Core.Entities;
using App.Core.Managers;
using App.Core.Models;
using App.UI.Infrastructure;
using MarminaAttendance.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace App.UI.Pages.Servant
{
    [Authorize(Roles = "SuperAdmin,Admin,ServiceAdmin")]
    public class EditcshtmlModel : PageModel
    {
        private readonly ServantManager servantManager;
        private readonly WeekManager weekManager;
        private readonly ClassManager classManager;
        private readonly ServiceManager serviceManager;
        private readonly int NumberOfWeeksAppearInMarkup = 16;
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public List<Weeks> WeeksList { get; private set; }
        [BindProperty]
        public Servants Servant { get; set; }
        [BindProperty]
        public ServantWeeksDTO ServantWeeksDTO { get; set; }
        public SelectList Classes { get; private set; }
        public SelectList Services { get; private set; }

        public EditcshtmlModel(ServantManager servantManager,
            WeekManager weekManager,
            ClassManager classManager,
            ServiceManager serviceManager)
        {
            this.servantManager = servantManager;
            this.weekManager = weekManager;
            this.classManager = classManager;
            this.serviceManager = serviceManager;
        }
        public async Task OnGet()
        {
           await FillDataAsync();
        }
        public async Task FillDataAsync()
        {

            WeeksList = weekManager.GetWeeks(NumberOfWeeksAppearInMarkup);
            Servant = servantManager.GetServant(Id);
            Classes = new SelectList(await classManager.GetFilteredClassesQueryAsync(), "Id", "Name");
            Services = new SelectList(serviceManager.GetServicesList(), "Id", "Name");
        }
        public async Task OnPost()
        {
            if (Servant.PhotoFile != null)
            {
                Servant.Photo = FileManager.UploadPhoto(Servant.PhotoFile, "wwwroot/photos/servant/", 285, 310);

            }
            var Result = servantManager.UpdateServant(Servant, ServantWeeksDTO);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم تحديث البيانات بنجاح" : Result.Error;
           await FillDataAsync();

        }
        public string GetFormattedWeekDate(DateTime week, string Time)
        {

            return classManager.GetFormattedWeekDate(week, Time);
        }
    }
}
