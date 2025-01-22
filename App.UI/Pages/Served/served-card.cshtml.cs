using App.Core.Managers;
using App.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages.Serveds
{
    [Authorize]
    public class printModel : PageModel
    {
        private readonly ServedManager _servedManager;
        [BindProperty(SupportsGet = true)] public int? classId { get; set; }

        public List<ServedVM> Serveds { get; set; }
        public printModel(ServedManager servedManager)
        {
            _servedManager = servedManager;
        }
        public void OnGet()
        {
            if (classId.HasValue && classId.Value > 0)
            {

                Serveds = _servedManager.GetServeds(classId.Value);
            }
        }
    }
}
