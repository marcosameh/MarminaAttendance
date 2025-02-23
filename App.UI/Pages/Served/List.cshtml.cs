using App.Core.Entities;
using App.Core.Managers;
using App.Core.Models;
using App.UI.Ifraustrcuture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AppCore.Common;
using Microsoft.AspNetCore.Identity;
using MarminaAttendance.Identity;

namespace App.UI.Pages.Serveds
{
    [Authorize]
    public class ListModel : PageModel
    {
        private readonly ServedManager ServedManager;
        private readonly ClassManager classManager;
        private readonly UserManager<ApplicationUser> userManager;

        public List<ServedVM> ServedsVM { get; private set; }
        public List<ClassVM> Classes { get; private set; }

        [BindProperty]
        public Served Served { get; set; }
        public ListModel(ServedManager ServedManager,
            ClassManager classManager,
            UserManager<ApplicationUser> userManager)
        {
            this.ServedManager = ServedManager;
            this.classManager = classManager;
            this.userManager = userManager;
        }
        public void OnGet()
        {
            FillData();
        }
        public IActionResult OnGetDisplayServeds()
        {
            var user = userManager.GetUserAsync(User).Result;
            ServedsVM = ServedManager.GetServeds(user.ClassId);

            return new JsonResult(ServedsVM);
        }

        public void OnPost()
        {
            FillData();
            if (Served.PhotoFile != null)
            {
                Served.Photo = FileManager.UploadPhoto(Served.PhotoFile, "/wwwroot/photos/المخدومين/", 285, 310);

            }
            var Result = ServedManager.AddServed(Served);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم اضافة المخدوم بنجاح" : Result.Error;
          
        }
        public void OnGetDelete(int id)
        {
            var Result = ServedManager.DeleteServed(id);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم مسح المخدوم " : Result.Error;
            FillData();
        }

        public void FillData()
        {
            var user = userManager.GetUserAsync(User).Result;
            Classes = classManager.GetClasses(user.ClassId);
        }
    }
}
