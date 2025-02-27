using App.Core.Managers;
using App.UI.InfraStructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages.Servant
{
    [Authorize]
    public class generate_QrModel : PageModel
    {
        private readonly ServantManager _ServantManager;
        private readonly QrCodeService _qrCodeService;

        public generate_QrModel(ServantManager ServantManager, QrCodeService qrCodeService)
        {
            _ServantManager = ServantManager;
            _qrCodeService = qrCodeService;
        }

        public async void OnGet()
        {
            var serveds = _ServantManager.GetServants();
            foreach (var served in serveds)
            {
                await _qrCodeService.GenerateQrCodeForServantsAsync(served.Id);
            }
        }
    }
}
