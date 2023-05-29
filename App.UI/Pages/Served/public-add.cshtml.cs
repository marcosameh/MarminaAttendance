using App.Core.Entities;
using App.Core.Managers;
using App.Core.Models;
using App.UI.Ifraustrcuture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages.Serveds
{
    public class public_addModel : PageModel
    {
        private readonly ClassManager classManager;
        private readonly ServedManager servedManager;

        [BindProperty]
        public Served Served { get; set; }
        public List<ClassVM> Classes { get; private set; }

        public public_addModel(ClassManager classManager,ServedManager servedManager)
        {
            this.classManager = classManager;
            this.servedManager = servedManager;
        }

        public void OnGet()
        {
            Classes = classManager.GetClasses();
        }
        public IActionResult OnPost()
        {
            Classes = classManager.GetClasses();

            if (Served.PhotoFile != null)
            {
                Served.Photo = FileManager.UploadPhoto(Served.PhotoFile, "/wwwroot/photos/المخدومين/", 285, 310);

            }
            var Result = servedManager.AddServed(Served);
            if(Result.IsSuccess)
            {
                return LocalRedirect($"/thank-you?name={Served.Name}");
            }
            TempData["NotificationType"] =  "error";
            TempData["Message"] = Result.Error;
            return Page();
        }
    }
}
