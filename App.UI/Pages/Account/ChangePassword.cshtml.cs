using MarminaAttendance.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace App.UI.Pages.Account
{
    [Authorize]
    public class ChangePasswordModel : PageModel
    {
        private readonly CustomUserManager _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ChangePasswordModel(
            CustomUserManager userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "كلمة المرور الحالية مطلوبة")]
            [DataType(DataType.Password)]
            [Display(Name = "كلمة المرور الحالية")]
            public string CurrentPassword { get; set; }

            [Required(ErrorMessage = "كلمة المرور الجديدة مطلوبة")]
            [StringLength(100, ErrorMessage = "يجب أن تتراوح كلمة المرور بين {2} و {1} حرفًا.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "كلمة المرور الجديدة")]
            public string NewPassword { get; set; }

            [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
            [DataType(DataType.Password)]
            [Display(Name = "تأكيد كلمة المرور")]
            [Compare("NewPassword", ErrorMessage = "كلمة المرور الجديدة وتأكيدها غير متطابقين")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("لم يتم العثور على المستخدم");
            }

            // Verify current password
            var checkPassword = await _userManager.CheckPasswordAsync(user, Input.CurrentPassword);
            if (!checkPassword)
            {
                TempData["NotificationType"] = "error";
                TempData["Message"] = "كلمة المرور الحالية غير صحيحة";
                return Page();
            }

            // Change password
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.CurrentPassword, Input.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                TempData["NotificationType"] = "error";
                TempData["Message"] = "فشل تغيير كلمة المرور";
                return Page();
            }

            // Update plain text password
            user.PlainPassword = Input.NewPassword;
            await _userManager.UpdateAsync(user);

            // Re-sign in user
            await _signInManager.RefreshSignInAsync(user);

            TempData["NotificationType"] = "success";
            TempData["Message"] = "تم تغيير كلمة المرور بنجاح";

            return RedirectToPage();
        }
    }
}
