using App.Core.Entities;
using App.Core.Managers;
using App.Core.Models;
using App.UI.Ifraustrcuture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace App.UI.Pages.Servant
{

    public class addModel : PageModel
    {
        private readonly ClassManager classManager;
        private readonly ServantManager servantManager;

        [BindProperty]
        public Servants Servant { get; set; }
        public List<ClassVM> Classes { get; private set; }

        public addModel(ClassManager classManager,
            ServantManager servantManager)
        {
            this.classManager = classManager;
            this.servantManager = servantManager;
        }
        public void OnGet()
        {
            FillData();
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
            if (TempData["NotificationType"] == "success")
            {
                 Redirect("servants/list");
            }
             
        }

        private void FillData()
        {
            Classes = classManager.GetClasses();
        }
    }
}
