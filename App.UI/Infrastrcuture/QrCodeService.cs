using Microsoft.AspNetCore.Hosting;
using QRCoder;
using System.IO;
using System.Threading.Tasks;

namespace App.UI.InfraStructure
{
    public class QrCodeService
    {
        private readonly IWebHostEnvironment _environment;

        public QrCodeService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> GenerateQrCodeForServedAsync(int servedId)
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode($"https://marmina.innovix.site/served/qr-attendance-registration?servedId={servedId}", QRCodeGenerator.ECCLevel.Q);
                using (var pngByteQRCode = new PngByteQRCode(qrCodeData))
                {
                    var qrCodeImage = pngByteQRCode.GetGraphic(20);
                    var fileName = $"{servedId}.png";
                    var relativePath = Path.Combine("photos/المخدومين/qr/", fileName);
                    var wwwrootPath = Path.Combine(_environment.WebRootPath, relativePath);

                    // Ensure the directory exists
                    var directory = Path.GetDirectoryName(wwwrootPath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    await File.WriteAllBytesAsync(wwwrootPath, qrCodeImage);

                    // Return the relative path to the saved image
                    return relativePath.Replace("\\", "/");
                }
            }
        }
        public async Task<string> GenerateQrCodeForServantsAsync(int servedId)
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode($"https://marmina.innovix.site/Servants/qr-attendance-registration?servantId={servedId}", QRCodeGenerator.ECCLevel.Q);
                using (var pngByteQRCode = new PngByteQRCode(qrCodeData))
                {
                    var qrCodeImage = pngByteQRCode.GetGraphic(20);
                    var fileName = $"{servedId}.png";
                    var relativePath = Path.Combine("photos/الخدام/qr/", fileName);
                    var wwwrootPath = Path.Combine(_environment.WebRootPath, relativePath);

                    // Ensure the directory exists
                    var directory = Path.GetDirectoryName(wwwrootPath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    await File.WriteAllBytesAsync(wwwrootPath, qrCodeImage);

                    // Return the relative path to the saved image
                    return relativePath.Replace("\\", "/");
                }
            }
        }
    }
}
