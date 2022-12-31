// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using MarminaAttendance.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MarminaAttendance.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment environment;
        private readonly ILogger<RegisterModel> _logger;

   

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterModel> logger
            )
        {
            _userManager = userManager;
            this.environment = environment;
            _signInManager = signInManager;
            this.roleManager = roleManager;
            _logger = logger;
            
        }


        [BindProperty]
        public InputModel Input { get; set; }


        public class InputModel
        {
            [Required]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }


            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public IFormFile Photo { get; set; }
        }



        public async Task<IActionResult> OnPostAsync()
        {


            if (ModelState.IsValid)
            {
                string folderName = "photos/users";
                string webRootPath = environment.WebRootPath;
                string FolderPath = Path.Combine(webRootPath, folderName);
                string photoname = Path.GetFileName(Input.Photo.FileName);
                string finalPath= Path.Combine(FolderPath, photoname);
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }             
                using (var stream = System.IO.File.Create(finalPath))
                {
                    await Input.Photo.CopyToAsync(stream);
                }

                var user = new ApplicationUser { UserName = Input.UserName, Email = Input.Email, Photo = photoname};
         
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                   // await roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _userManager.AddToRoleAsync(user, "Admin");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect("/index");

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
