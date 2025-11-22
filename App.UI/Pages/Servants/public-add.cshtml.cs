using App.Core.Entities;
using App.Core.Managers;
using App.Core.Models;
using App.UI.Infrastructure;
using App.UI.InfraStructure;
using AppCore.Common;
using MarminaAttendance.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages.Servant
{
    public class public_addModel : PageModel
    {
        private readonly ServantManager _servantManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly QrCodeService _qrCodeService;
        private readonly IWebHostEnvironment environment;

        [BindProperty]
        public Servants Servant { get; set; }
        public ClassManager ClassManager { get; }
        public List<ClassVM> Classes { get; private set; }


        public public_addModel(ClassManager classManager,
            ServantManager servantManager,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            QrCodeService qrCodeService,
            IWebHostEnvironment environment)
        {
            ClassManager = classManager;
            _servantManager = servantManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _qrCodeService = qrCodeService;
            this.environment = environment;
        }
        public void OnGet()
        {
            Classes = ClassManager.GetClasses();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Classes = ClassManager.GetClasses();

            if (Servant.PhotoFile != null)
            {
                Servant.Photo = FileManager.UploadPhoto(Servant.PhotoFile, "/wwwroot/photos/المخدومين/", 285, 310);

            }
            var Result = _servantManager.AddServant(Servant);
            if (Result.IsSuccess)
            {
                await _qrCodeService.GenerateQrCodeForServantsAsync(Servant.Id);
                return LocalRedirect($"/thank-you?name={Servant.Name}");
            }
            TempData["NotificationType"] = "error";
            TempData["Message"] = Result.Error;
            return Page();
        }
    }
}
