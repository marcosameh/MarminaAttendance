using Microsoft.AspNetCore.Mvc;

namespace App.UI.API
{
    public class ClassController : Controller
    {
        public ClassController()
        {
                
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
