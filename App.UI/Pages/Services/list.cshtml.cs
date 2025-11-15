using App.Core.Entities;
using App.Core.Managers;
using App.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages.Service
{
    [Authorize]
    public class listModel : PageModel
    {
        private readonly ServiceManager serviceManager;

        public List<ServiceVM> Services { get; set; }
        [BindProperty]
        public App.Core.Entities.Services serviceData { get; set; }

        public listModel(ServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }

        public void OnGet()
        {
        }

        public IActionResult OnGetDisplayServices()
        {
            Services = serviceManager.GetServices();
            return new JsonResult(Services);
        }

        public void OnPost()
        {
            var Result = serviceManager.AddService(serviceData);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم اضافة الخدمة بنجاح" : Result.Error;
        }

        public void OnGetDelete(int id)
        {
            var Result = serviceManager.DeleteService(id);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم مسح الخدمة بنجاح" : Result.Error;
        }
    }
}
