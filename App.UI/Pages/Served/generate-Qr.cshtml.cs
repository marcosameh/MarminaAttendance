using App.Core.Managers;
using App.UI.InfraStructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages.Serveds
{
    [Authorize]
    public class generate_QrModel : PageModel
    {
        private readonly ServedManager _servedManager;
        private readonly QrCodeService _qrCodeService;

        public generate_QrModel(ServedManager servedManager, QrCodeService qrCodeService)
        {
            _servedManager = servedManager;
            _qrCodeService = qrCodeService;
        }

        public async void OnGet()
        {
            var serveds = _servedManager.GetServeds();
            foreach (var served in serveds)
            {
                await _qrCodeService.GenerateQrCodeForServedAsync(served.Id);
            }
        }
    }
}
