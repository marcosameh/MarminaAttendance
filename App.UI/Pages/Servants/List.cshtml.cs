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

namespace App.UI.Pages.Servant
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ListModel : PageModel
    {
        private readonly ServantManager servantManager;
        private readonly ClassManager classManager;
        private readonly ServiceManager serviceManager;
        private readonly CustomUserManager userManager;
        private readonly QrCodeService qrCodeService;

        public IQueryable<ServantVM> ServantsVM { get; private set; }
        public IQueryable<ClassVM> Classes { get; private set; }
        public List<Services> Services { get; private set; }
        public List<ServantVM> ServiceAdmins { get; private set; }

        [BindProperty]
        public Servants Servant { get; set; }

        public ListModel(
            ServantManager servantManager,
            ClassManager classManager,
            ServiceManager serviceManager,
            CustomUserManager userManager,
            QrCodeService qrCodeService)
        {
            this.servantManager = servantManager;
            this.classManager = classManager;
            this.serviceManager = serviceManager;
            this.userManager = userManager;
            this.qrCodeService = qrCodeService;
        }

        public async Task OnGetAsync()
        {
            await FillDataAsync();
        }

        public async Task<IActionResult> OnGetDisplayServants()
        {
            ServantsVM = await servantManager.GetFilteredServantsQueryAsync();
            return new JsonResult(ServantsVM);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await FillDataAsync();

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

            // Create Application User - Username is auto-generated from Name
            var user = new ApplicationUser
            {
                UserName = Servant.Name,
                Photo = Servant.Photo,
                ServantId = servantResult.Value,
                PlainPassword = Servant.Password, // Store plain text password
            };

            var userCreationResult = await userManager.CreateAsync(user, Servant.Password);

            if (!userCreationResult.Succeeded)
            {
                foreach (var error in userCreationResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                TempData["NotificationType"] = "error";
                TempData["Message"] = string.Join(", ", userCreationResult.Errors.Select(e => e.Description));
                return Page();
            }

            // Assign Role & Generate QR Code
            if (Servant.ClassId.HasValue && Servant.ClassId > 0)
            {
                await userManager.AddToRoleAsync(user, "Servant");

            }
            if (Servant.ServiceId.HasValue && Servant.ServiceId > 0)
            {
                await userManager.AddToRoleAsync(user, "ServiceAdmin");

            }
            await qrCodeService.GenerateQrCodeForServantsAsync(Servant.Id);

            return RedirectToPage();
        }

        public async Task OnGetDeleteAsync(int id)
        {
            await FillDataAsync();
            // First, find and delete the associated user
            var user = await userManager.FindByServantIdAsync(id);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
            }

            // Then delete the servant
            var Result = servantManager.DeleteServant(id);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم مسح الخادم" : Result.Error;
        }

        private async Task FillDataAsync()
        {
            Classes = await classManager.GetFilteredClassesQueryAsync();
            Services = serviceManager.GetServicesList();
            ServiceAdmins = servantManager.GetServiceAdmins();
        }
    }
}
