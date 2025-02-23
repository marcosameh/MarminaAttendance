using App.Core.Entities;
using App.Core.Managers;
using App.Core.Models;
using App.UI.Ifraustrcuture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AppCore.Common;
using Microsoft.AspNetCore.Identity;
using MarminaAttendance.Identity;

namespace App.UI.Pages.Servant
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ListModel : PageModel
    {
        private readonly ServantManager servantManager;
        private readonly UserManager<ApplicationUser> userManager;

        public List<ServantVM> ServantsVM { get; private set; }


        public ListModel(ServantManager servantManager,
            UserManager<ApplicationUser> userManager)
        {
            this.servantManager = servantManager;
            this.userManager = userManager;
        }
        public void OnGet()
        {

        }
        public IActionResult OnGetDisplayServants()
        {
            var user = userManager.GetUserAsync(User).Result;
            ServantsVM = servantManager.GetServants(user.ClassId);

            return new JsonResult(ServantsVM);
        }


        public void OnGetDelete(int id)
        {
            var Result = servantManager.DeleteServant(id);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم مسح الخادم" : Result.Error;

        }


    }
}
