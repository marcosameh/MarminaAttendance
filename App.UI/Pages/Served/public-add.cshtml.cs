using App.Core.Entities;
using App.Core.Managers;
using App.Core.Models;
using App.UI.Infrastructure;
using App.UI.InfraStructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages.Serveds
{
    public class public_addModel : PageModel
    {
        private readonly ClassManager classManager;
        private readonly ServedManager servedManager;
        private readonly QrCodeService qrCodeService;

        [BindProperty]
        public Served Served { get; set; }
        public List<ClassVM> Classes { get; private set; }

        public public_addModel(ClassManager classManager,
            ServedManager servedManager,
            QrCodeService qrCodeService)
        {
            this.classManager = classManager;
            this.servedManager = servedManager;
            this.qrCodeService = qrCodeService;
        }

        public void OnGet()
        {
            Classes = classManager.GetClasses();
        }
        public async Task<IActionResult> OnPost()
        {
            Classes = classManager.GetClasses();

            if (Served.PhotoFile != null)
            {
                Served.Photo = FileManager.UploadPhoto(Served.PhotoFile, "/wwwroot/photos/المخدومين/", 285, 310);

            }
            var Result = servedManager.AddServed(Served);
            if (Result.IsSuccess)
            {
                await qrCodeService.GenerateQrCodeForServedAsync(Served.Id);
                return LocalRedirect($"/thank-you?name={Served.Name}");
            }
            TempData["NotificationType"] = "error";
            TempData["Message"] = Result.Error;
            return Page();
        }
    }
}
