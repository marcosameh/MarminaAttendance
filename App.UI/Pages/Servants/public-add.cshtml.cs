using App.Core.Entities;
using App.Core.Managers;
using App.Core.Models;
using App.UI.Ifraustrcuture;
using AppCore.Common;
using MarminaAttendance.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace App.UI.Pages.Servant
{
    public class public_addModel : PageModel
    {
        private readonly ServantManager servantManager;

        [BindProperty]
        public Servants Servant { get; set; }
        public ClassManager ClassManager { get; }
        public List<ClassVM> Classes { get; private set; }

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment environment;
        public public_addModel(ClassManager classManager, ServantManager servantManager, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)
        {
            ClassManager = classManager;
            this.servantManager = servantManager;
            _signInManager = signInManager;
            _userManager = userManager;
            this.environment = environment;
        }
        public void OnGet()
        {
            Classes = ClassManager.GetClasses();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Classes = ClassManager.GetClasses();
            if (ModelState.IsValid)
            {

                string photoname = "user.jpg";
                if (Servant.PhotoFile != null)
                {
                    Servant.Photo = FileManager.UploadPhoto(Servant.PhotoFile, "/wwwroot/photos/الخدام/", 285, 310);
                    photoname = FileManager.UploadPhoto(Servant.PhotoFile, "/wwwroot/photos/users/", 285, 310);

                }
                var user = new ApplicationUser { UserName = Servant.Name, Photo = photoname };

                var result = await _userManager.CreateAsync(user, Servant.Password);

                if (result.Succeeded)
                {


                    //await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                    await _userManager.AddToRoleAsync(user, "Admin");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    var Result = servantManager.AddServant(Servant);
                    TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
                    TempData["Message"] = Result.IsSuccess ? "تم اضافة الخادم بنجاح" : Result.Error;
                    return LocalRedirect("/classes/list");

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }




            }
            // If we got this far, something failed, redisplay form

            return Page();



        }
    }
}
