using App.Core.Entities;
using App.Core.Managers;
using App.Core.Models;
using App.UI.Ifraustrcuture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AppCore.Common;

namespace App.UI.Pages.Servant
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ListModel : PageModel
    {
        private readonly ServantManager servantManager;
        private readonly ClassManager classManager;

        public List<ServantVM> ServantsVM { get; private set; }
        public List<ClassVM> Classes { get; private set; }

        [BindProperty]
        public Servants Servant { get; set; }
        public ListModel(ServantManager servantManager, ClassManager classManager)
        {
            this.servantManager = servantManager;
            this.classManager = classManager;
        }
        public void OnGet()
        {
            FillData();
        }
        public IActionResult OnGetDisplayServants()
        {
            ServantsVM = servantManager.GetServants();

            return new JsonResult(ServantsVM);
        }

        public void OnPost()
        {
            FillData();
            if (Servant.PhotoFile != null)
            {
                Servant.Photo = FileManager.UploadPhoto(Servant.PhotoFile, "/wwwroot/photos/الخدام/", 285, 310);

            }
            var Result = servantManager.AddServant(Servant);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم اضافة الخادم  بنجاح" : Result.Error;
          
        }
        public void OnGetDelete(int id)
        {
            var Result = servantManager.DeleteServant(id);

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
