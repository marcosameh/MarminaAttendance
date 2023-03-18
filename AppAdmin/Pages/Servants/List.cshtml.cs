using App.Core.Entities;
using App.Core.Managers;
using App.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages.Servant
{
    public class ListModel : PageModel
    {
        private readonly ServantManager servantManager;
        private readonly ClassManager classManager;

        public List<ServantVM> ServantsVM { get; private set; }
        public List<ClassVM> Classes { get; private set; }

        [BindProperty] 
        public Servants Servant { get; set; }
        public ListModel(ServantManager servantManager,ClassManager classManager)
        {
            this.servantManager = servantManager;
            this.classManager = classManager;
        }
        public void OnGet()
        {
            FillData();
        }
        public IActionResult OnGetDisplayServants()
        {
            ServantsVM = servantManager.GetServants();

            return new JsonResult(ServantsVM);
        }
        public void FillData()
        {
            Classes=classManager.GetClasses();
        }
    }
}
