using App.Core.Managers;
using App.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages.Classes
{
    public class listModel : PageModel
    {
        private readonly ClassManager classManager;

        public List<ClassVM> Classes { get; set; }
        public listModel(ClassManager classManager)
        {
            this.classManager = classManager;
        }
        //public void OnGet()
        //{
        //    Classes=classManager.GetClasses();
        //}
        public IActionResult OnGetDisplayClasses()
        {
            Classes = classManager.GetClasses();
          
            return new JsonResult(Classes);
        }
    }
}
