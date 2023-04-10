using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages.Servant
{
    [Authorize]
    public class quick_attendance_registrationModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
