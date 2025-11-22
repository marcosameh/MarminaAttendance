using App.Core.Managers;
using AppCore.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace App.UI.Pages.Serveds
{
    [Authorize]
    public class quick_attendance_registrationModel : PageModel
    {
        private readonly ServedManager ServedManager;
        [BindProperty(SupportsGet = true)]
        public int ServedId { get; set; }
        public quick_attendance_registrationModel(ServedManager ServedManager)
        {
            this.ServedManager = ServedManager;
        }

        public void OnGet()
        {
        }


        public IActionResult OnGetSearchServeds(string searchInput)
        {
            var result = ServedManager.SearchServeds(searchInput);
            if (result.IsSuccess)
            {
                return new JsonResult(result.Value);
            }
            return BadRequest(result.Error);
        }
        public async Task OnPost()
        {

            var Result =await ServedManager.AttendanceRegistrationAsync(ServedId);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? Result.Value : Result.Error;

        }

    }
}
