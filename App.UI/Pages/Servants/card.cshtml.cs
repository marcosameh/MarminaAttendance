using App.Core.Entities;
using App.Core.Managers;
using App.Core.Models;
using MarminaAttendance.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages.Servant
{
    [Authorize]
    public class printModel : PageModel
    {
        private readonly ServantManager _servantManager;

        [BindProperty(SupportsGet = true)] public int? servantId { get; set; }

        public Servants Servant { get; set; }
     
        public printModel(
            ServantManager servantManager)

        {
           
            _servantManager = servantManager;
        }
        public void OnGet()
        {
            if (servantId.HasValue && servantId.Value > 0)
            {               
                Servant = _servantManager.GetServant(servantId.Value);
               
            }
        }
    }
}
