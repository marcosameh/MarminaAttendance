using App.Core.Managers;
using App.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AppCore.Common;
using App.UI.Ifraustrcuture;

namespace App.UI.Pages.Servant
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class quick_attendance_registrationModel : PageModel
    {
        private readonly ServantManager servantManager;
        [BindProperty(SupportsGet =true)]
        public int ServantId { get; set; }
        public quick_attendance_registrationModel(ServantManager servantManager)
        {
            this.servantManager = servantManager;
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
            TempData["Message"] = Result.IsSuccess? "تم تسحيل حضور الخادم" : Result.Error;

        }

    }
}
