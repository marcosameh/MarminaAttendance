using App.Core.Entities;
using App.Core.Managers;
using App.Core.Models;
using App.UI.Ifraustrcuture;
using App.UI.InfraStructure;
using MarminaAttendance.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace App.UI.Pages.Servant
{

    public class addModel : PageModel
    {
        private readonly ClassManager classManager;
        private readonly ServantManager servantManager;
        private readonly CustomUserManager userManager;
        private readonly QrCodeService qrCodeService;
        private readonly ServiceManager serviceManager;

        [BindProperty]
        public Servants Servant { get; set; }
        public IQueryable<ClassVM> Classes { get; private set; }
        public List<Services> Services { get; private set; }

        public addModel(ClassManager classManager,
            ServantManager servantManager,
            CustomUserManager userManager,
             RoleManager<IdentityRole> roleManager,
             QrCodeService qrCodeService,
             ServiceManager serviceManager)

        {
            this.classManager = classManager;
            this.servantManager = servantManager;
            this.userManager = userManager;
            this.qrCodeService = qrCodeService;
            this.serviceManager = serviceManager;
        }
        public async Task OnGet()
        {
            await FillData();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await FillData();

            // Handle Photo Upload
            if (Servant.PhotoFile != null)
            {
                Servant.Photo = FileManager.UploadPhoto(Servant.PhotoFile, "/wwwroot/photos/الخدام/", 285, 310);
            }

            // Add Servant to the system
            var servantResult = servantManager.AddServant(Servant);

            TempData["NotificationType"] = servantResult.IsSuccess ? "success" : "error";
            TempData["Message"] = servantResult.IsSuccess ? "تم اضافة الخادم بنجاح" : servantResult.Error;

            if (!servantResult.IsSuccess)
            {
                return Page();
            }

            // Create Application User
            var user = new ApplicationUser
            {
                UserName = Servant.Name,
                Photo = Servant.Photo,
                ServantId = Servant.Id,
            };

            var userCreationResult = await userManager.CreateAsync(user, Servant.Password);

            if (!userCreationResult.Succeeded)
            {
                foreach (var error in userCreationResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            // Assign Role & Sign in User
            await userManager.AddToRoleAsync(user, "Servant");
            await qrCodeService.GenerateQrCodeForServantsAsync(Servant.Id);

            return LocalRedirect("/Servants/list");
        }


        private async Task FillData()
        {
            Classes = await classManager.GetFilteredClassesQueryAsync();
            Services = serviceManager.GetServicesList();
        }
    }
}
