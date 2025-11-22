using App.Core.Managers;
using AppCore.Common;
using MarminaAttendance.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages.Servant
{
    [Authorize(Roles = "SuperAdmin,Admin,ServiceAdmin")]
    public class quick_attendance_registrationModel : PageModel
    {
        private readonly ServantManager servantManager;
        private readonly CustomUserManager customUserManager;

        [BindProperty(SupportsGet = true)]
        public int ServantId { get; set; }
        public quick_attendance_registrationModel(ServantManager servantManager,
            CustomUserManager customUserManager)

        {
            this.servantManager = servantManager;
            this.customUserManager = customUserManager;
        }

        public void OnGet()
        {
        }


        public IActionResult OnGetSearchServants(string searchInput)
        {
            var result = servantManager.SearchServants(searchInput);
            if (result.IsSuccess)
            {
                return new JsonResult(result.Value);
            }
            return BadRequest(result.Error);
        }
        public void OnPost()
        {

            var Result = servantManager.AttendanceRegistration(ServantId);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم تسحيل حضور الخادم" : Result.Error;

        }

    }
}
