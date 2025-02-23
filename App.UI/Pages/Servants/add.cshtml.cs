using App.Core.Entities;
using App.Core.Managers;
using App.Core.Models;
using App.UI.Ifraustrcuture;
using MarminaAttendance.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace App.UI.Pages.Servant
{

    public class addModel : PageModel
    {
        private readonly ClassManager classManager;
        private readonly ServantManager servantManager;
        private readonly UserManager<ApplicationUser> userManager;

        [BindProperty]
        public Servants Servant { get; set; }
        public List<ClassVM> Classes { get; private set; }

        public addModel(ClassManager classManager,
            ServantManager servantManager,
            UserManager<ApplicationUser> userManager,
             RoleManager<IdentityRole> roleManager)

        {
            this.classManager = classManager;
            this.servantManager = servantManager;
            this.userManager = userManager;
        }
        public void OnGet()
        {
            FillData();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            FillData();

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
                return Page(); // Return the page with an error message
            }

            // Create Application User
            var user = new ApplicationUser
            {
                UserName = Servant.Name,
                Photo = Servant.Photo,
                ClassId = Servant.ClassId
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
            await userManager.AddToRoleAsync(user,"Servant");
         

            return LocalRedirect("/classes/list");
        }


        private void FillData()
        {
            var user = userManager.GetUserAsync(User).Result;
            Classes = classManager.GetClasses(user.ClassId);
        }
    }
}
