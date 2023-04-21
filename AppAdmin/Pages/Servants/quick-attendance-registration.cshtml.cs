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


    }
}
