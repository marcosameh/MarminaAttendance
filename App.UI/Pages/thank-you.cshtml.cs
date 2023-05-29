using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages
{
    public class thank_youModel : PageModel
    {
        [BindProperty(SupportsGet =true)]
        public string Name { get; set; }
        public void OnGet()
        {
        }
    }
}
