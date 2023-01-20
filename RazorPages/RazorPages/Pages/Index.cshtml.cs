using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QRCoder;

namespace RazorPages.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public string ImageSrc { get; set; } = string.Empty;

    public IActionResult OnGet()
    {
        _logger.LogInformation("LOG TEST");
        var text = "https://phasetr.com/archive";
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);
        var bytes = qrCode.GetGraphic(10);
        var base64Str = Convert.ToBase64String(bytes);
        ImageSrc = $"data:image/png;base64,{base64Str}";
        return Page();
    }
}