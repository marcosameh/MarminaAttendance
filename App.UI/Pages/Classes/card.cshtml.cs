using App.Core.Managers;
using App.Core.Models;
using Microsoft.AspNetCore.Authorization;
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

        public IQueryable<ServedVM> Serveds { get; set; }
        public IQueryable<ServantVM> Servants { get; set; }
        public printModel(ServedManager servedManager,
            ServantManager servantManager)

        {
            _servedManager = servedManager;
            _servantManager = servantManager;
        }
        public async Task OnGet()
        {
            if (classId.HasValue && classId.Value > 0)
            {
                Servants = await _servantManager.GetFilteredServantsQueryAsync();
                Serveds =await _servedManager.GetFilteredServedsQueryAsync();
            }
        }
    }
}
