using App.Core.Entities;
using App.Core.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages.Service
{
    [Authorize]
    public class editModel : PageModel
    {
        private readonly ServiceManager serviceManager;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public App.Core.Entities.Services CurrentService { get; set; }

        public editModel(ServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }

        public void OnGet()
        {
            FillData();
        }

        public IActionResult OnPost()
        {
            var Result = serviceManager.UpdateService(CurrentService);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم تحديث البيانات بنجاح" : Result.Error;
            FillData();

            return Page();
        }

        public void FillData()
        {
            CurrentService = serviceManager.GetService(Id);
        }
    }
}
