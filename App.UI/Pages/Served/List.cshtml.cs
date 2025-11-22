using App.Core.Entities;
using App.Core.Managers;
using App.Core.Models;
using App.UI.Infrastructure;
using App.UI.InfraStructure;
using AppCore.Common;
using MarminaAttendance.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages.Serveds
{
    [Authorize]
    public class ListModel : PageModel
    {
        private readonly ServedManager ServedManager;
        private readonly ClassManager classManager;
        private readonly QrCodeService qrCodeService;

        public IQueryable<ServedVM> ServedsVM { get; private set; }
        public IQueryable<ClassVM> Classes { get; private set; }

        [BindProperty]
        public Served Served { get; set; }
        public ListModel(ServedManager ServedManager,
            ClassManager classManager,
            CustomUserManager userManager,
            QrCodeService qrCodeService)
        {
            this.ServedManager = ServedManager;
            this.classManager = classManager;
            this.qrCodeService = qrCodeService;
        }
        public async Task OnGetAsync()
        {
            await FillDataAsync();
        }
        public async Task<IActionResult> OnGetDisplayServedsAsync()
        {
            ServedsVM = await ServedManager.GetFilteredServedsQueryAsync();

            return new JsonResult(ServedsVM);
        }

        public async void OnPost()
        {
            await FillDataAsync();
            if (Served.PhotoFile != null)
            {
                Served.Photo = FileManager.UploadPhoto(Served.PhotoFile, "/wwwroot/photos/المخدومين/", 285, 310);

            }
            var Result = ServedManager.AddServed(Served);
            if (Result.IsSuccess)
            {
                await qrCodeService.GenerateQrCodeForServedAsync(Served.Id);
            }
            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم اضافة المخدوم بنجاح" : Result.Error;

        }
        public void OnGetDelete(int id)
        {
            var Result = ServedManager.DeleteServed(id);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم مسح المخدوم " : Result.Error;
            FillDataAsync();
        }

        public async Task FillDataAsync()
        {
            Classes = await classManager.GetFilteredClassesQueryAsync();
        }
    }
}
