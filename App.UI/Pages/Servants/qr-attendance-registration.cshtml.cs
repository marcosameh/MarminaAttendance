using App.Core.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages.Servant
{
    [Authorize(Roles = "SuperAdmin,Admin,ServiceAdmin")]
    public class qr_attendance_registrationModel : PageModel
    {
        private readonly ServantManager _ServantManager;
        [BindProperty(SupportsGet =true)] public int? servantId { get; set; }
        public qr_attendance_registrationModel(ServantManager ServantManager)
        {
            _ServantManager = ServantManager;
        }
        public void OnGet()
        {
            if(servantId.HasValue && servantId.Value > 0)
            {
                var Result = _ServantManager.AttendanceRegistration(servantId.Value);

                TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
                TempData["Message"] = Result.IsSuccess ? Result.Value : Result.Error;
            }
            
        }
    }
}
