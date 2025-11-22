using MarminaAttendance.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace App.UI.Pages.Users
{
    [Authorize(Roles = "SuperAdmin")]
    public class ListModel : PageModel
    {
        private readonly CustomUserManager _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ListModel(
            CustomUserManager userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "اسم المستخدم مطلوب")]
            [Display(Name = "اسم المستخدم")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "كلمة المرور مطلوبة")]
            [StringLength(100, ErrorMessage = "يجب أن تتراوح كلمة المرور بين {2} و {1} حرفًا.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "كلمة المرور")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "تأكيد كلمة المرور")]
            [Compare("Password", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقين")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "الدور مطلوب")]
            [Display(Name = "الدور")]
            public string Role { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetDisplayUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var roleName = roles.FirstOrDefault() ?? "No Role";

                userList.Add(new
                {
                    id = user.Id,
                    userName = user.UserName,
                    role = roleName,
                    roleArabic = GetArabicRoleName(roleName),
                    isSuperAdmin = roleName == "SuperAdmin",
                    roleOrder = GetRoleOrder(roleName)
                });
            }

            // Sort by role order: SuperAdmin -> Admin -> ServiceAdmin -> Servant
            var sortedList = userList.OrderBy(u => ((dynamic)u).roleOrder).ToList();

            return new JsonResult(sortedList);
        }

        private int GetRoleOrder(string role)
        {
            return role switch
            {
                "SuperAdmin" => 1,
                "Admin" => 2,
                "ServiceAdmin" => 3,
                "Servant" => 4,
                _ => 5
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["NotificationType"] = "error";
                TempData["Message"] = "يرجى التحقق من البيانات المدخلة";
                return Page();
            }

            var user = new ApplicationUser
            {
                UserName = Input.UserName,
                PlainPassword = Input.Password
            };

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Input.Role);
                TempData["NotificationType"] = "success";
                TempData["Message"] = "تم إضافة المستخدم بنجاح";
                return RedirectToPage();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            TempData["NotificationType"] = "error";
            TempData["Message"] = string.Join(", ", result.Errors.Select(e => e.Description));
            return Page();
        }

        public async Task<IActionResult> OnGetDeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["NotificationType"] = "error";
                TempData["Message"] = "معرف المستخدم غير صالح";
                return RedirectToPage();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["NotificationType"] = "error";
                TempData["Message"] = "المستخدم غير موجود";
                return RedirectToPage();
            }

            // Check if user is SuperAdmin
            var isSuperAdmin = await _userManager.IsInRoleAsync(user, "SuperAdmin");
            if (isSuperAdmin)
            {
                TempData["NotificationType"] = "error";
                TempData["Message"] = "لا يمكن حذف مستخدم SuperAdmin";
                return RedirectToPage();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["NotificationType"] = "success";
                TempData["Message"] = "تم حذف المستخدم بنجاح";
            }
            else
            {
                TempData["NotificationType"] = "error";
                TempData["Message"] = "فشل حذف المستخدم";
            }

            return RedirectToPage();
        }

        private string GetArabicRoleName(string role)
        {
            return role switch
            {
                "SuperAdmin" => "مدير النظام",
                "Admin" => "مشرف",
                "Servant" => "خادم",
                "ServiceAdmin" => "أمين خدمة",
                _ => role
            };
        }
    }
}
