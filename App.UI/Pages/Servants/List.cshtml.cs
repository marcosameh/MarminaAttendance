using App.Core.Entities;
using App.Core.Managers;
using App.Core.Models;
using App.UI.Ifraustrcuture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AppCore.Common;

namespace App.UI.Pages.Servant
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ListModel : PageModel
    {
        private readonly ServantManager servantManager;


        public List<ServantVM> ServantsVM { get; private set; }


        public ListModel(ServantManager servantManager)
        {
            this.servantManager = servantManager;

        }
        public void OnGet()
        {

        }
        public IActionResult OnGetDisplayServants()
        {
            ServantsVM = servantManager.GetServants();

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
