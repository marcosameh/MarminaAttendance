using App.Core.Managers;
using App.Core.Models;
using MarminaAttendance.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages.Classes
{
    [Authorize]
    public class printModel : PageModel
    {
        private readonly ServedManager _servedManager;
        private readonly ServantManager _servantManager;

        [BindProperty(SupportsGet = true)] public int? classId { get; set; }

        public List<ServedVM> Serveds { get; set; }
        public List<ServantVM> Servants { get; set; }
        public printModel(ServedManager servedManager,
            ServantManager servantManager)

        {
            _servedManager = servedManager;
            _servantManager = servantManager;
        }
        public void OnGet()
        {
            if (classId.HasValue && classId.Value > 0)
            {               
                Servants = _servantManager.GetServants(classId.Value);
                Serveds = _servedManager.GetServeds(classId.Value);
            }
        }
    }
}
