using App.Core.Entities;
using App.Core.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages.Serveds
{
    [Authorize]
    public class printModel : PageModel
    {
        private readonly ServedManager _ServedManager;

        [BindProperty(SupportsGet = true)] public int? servedId { get; set; }

        public Served Served { get; set; }

        public printModel(
            ServedManager servedManager)

        {

            _ServedManager = servedManager;
        }
        public void OnGet()
        {
            if (servedId.HasValue && servedId.Value > 0)
            {
                Served = _ServedManager.GetServed(servedId.Value);


            }
        }
    }
}
