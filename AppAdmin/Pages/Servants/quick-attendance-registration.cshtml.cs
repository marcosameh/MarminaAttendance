using App.Core.Managers;
using App.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AppCore.Common;

namespace App.UI.Pages.Servant
{
    [Authorize]
    public class quick_attendance_registrationModel : PageModel
    {
        private readonly ServantManager servantManager;

        public quick_attendance_registrationModel(ServantManager servantManager)
        {
            this.servantManager = servantManager;
        }

        public Result<ServantVM> ServantsVM { get; private set; }

        public void OnGet()
        {
        }


        public IActionResult OnGetSearchServants(string searchInput)
        {
            ServantsVM = servantManager.SearchServants(searchInput);

            return new JsonResult(ServantsVM);
        }


    }
}
