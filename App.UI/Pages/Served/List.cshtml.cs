using App.Core.Entities;
using App.Core.Managers;
using App.Core.Models;
using App.UI.Ifraustrcuture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AppCore.Common;

namespace App.UI.Pages.Serveds
{
    [Authorize]
    public class ListModel : PageModel
    {
        private readonly ServedManager ServedManager;
        private readonly ClassManager classManager;

        public List<ServedVM> ServedsVM { get; private set; }
        public List<ClassVM> Classes { get; private set; }

        [BindProperty]
        public Served Served { get; set; }
        public ListModel(ServedManager ServedManager, ClassManager classManager)
        {
            this.ServedManager = ServedManager;
            this.classManager = classManager;
        }
        public void OnGet()
        {
            FillData();
        }
        public IActionResult OnGetDisplayServeds()
        {
            ServedsVM = ServedManager.GetServeds();

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
            TempData["Message"] = Result.IsSuccess ? "تم اضافة الخادم  بنجاح" : Result.Error;
          
        }
        public void OnGetDelete(int id)
        {
            var Result = ServedManager.DeleteServed(id);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم مسح الخادم" : Result.Error;
            FillData();
        }

        public void FillData()
        {
            Classes = classManager.GetClasses();
        }
    }
}
