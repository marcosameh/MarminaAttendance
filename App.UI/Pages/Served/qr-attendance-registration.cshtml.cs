using App.Core.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages.Serveds
{
    [Authorize]
    public class qr_attendance_registrationModel : PageModel
    {
        private readonly ServedManager servedManager;
        [BindProperty(SupportsGet =true)] public int?servedId { get; set; }
        public qr_attendance_registrationModel(ServedManager ServedManager)
        {
            servedManager = ServedManager;
        }
        public void OnGet()
        {
            if(servedId.HasValue &&servedId.Value > 0)
            {
                var Result = servedManager.RegisterAttendance(servedId.Value);

                TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
                TempData["Message"] = Result.IsSuccess ? Result.Value : Result.Error;
            }
            
        }
    }
}
