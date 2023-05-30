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
        public IActionResult OnPost()
        {
            Classes = ClassManager.GetClasses();

            if (Servant.PhotoFile != null)
            {
                Servant.Photo = FileManager.UploadPhoto(Servant.PhotoFile, "/wwwroot/photos/المخدومين/", 285, 310);

            }
            var Result = servantManager.AddServant(Servant);
            if (Result.IsSuccess)
            {
                return LocalRedirect($"/thank-you?name={Servant.Name}");
            }
            TempData["NotificationType"] = "error";
            TempData["Message"] = Result.Error;
            return Page();
        }
    }
}
