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
using System.Threading.Tasks;

namespace App.UI.Pages.Serveds
{
    [Authorize]
    public class ListModel(ServedManager ServedManager,
        ClassManager classManager,
        QrCodeService qrCodeService) : PageModel
    {
        private readonly ServedManager ServedManager = ServedManager;
        private readonly ClassManager classManager = classManager;
        private readonly QrCodeService qrCodeService = qrCodeService;

        public IQueryable<ServedVM> ServedsVM { get; private set; }
        public IQueryable<ClassVM> Classes { get; private set; }

        [BindProperty]
        public Served Served { get; set; }

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
                Served.Photo = FileManager.UploadPhoto(Served.PhotoFile, "wwwroot/photos/Served/", 285, 310);

            }
            var Result = ServedManager.AddServed(Served);
            if (Result.IsSuccess)
            {
                await qrCodeService.GenerateQrCodeForServedAsync(Served.Id);
            }
            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم اضافة المخدوم بنجاح" : Result.Error;

        }
        public async Task OnGetDelete(int id)
        {
            var Result = ServedManager.DeleteServed(id);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم مسح المخدوم " : Result.Error;
            await FillDataAsync();
        }

        public async Task FillDataAsync()
        {
            Classes = await classManager.GetFilteredClassesQueryAsync();
        }
    }
}
