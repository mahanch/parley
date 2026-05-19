using Microsoft.AspNetCore.Mvc;
using Parley.Application.Contracts.Interfaces.Infrastructure;

namespace Parley.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttachmentsController : ControllerBase
{
    private readonly IStorageService _storageService;

    public AttachmentsController(IStorageService storageService)
    {
        _storageService = storageService;
    }

    [HttpPost("generate-upload-url")]
    public async Task<IActionResult> GenerateUploadUrl([FromBody] GenerateUploadUrlRequest request, CancellationToken cancellationToken)
    {
        // ۱. اعتبارسنجی اولیه
        if (string.IsNullOrWhiteSpace(request.FileName) || string.IsNullOrWhiteSpace(request.ContentType))
        {
            return BadRequest(new { Error = "FileName and ContentType are required." });
        }

        // اختیاری: محدود کردن فرمت‌ها به عکس و ویدیو
        if (!request.ContentType.StartsWith("image/") && !request.ContentType.StartsWith("video/"))
        {
            return BadRequest(new { Error = "Only image and video uploads are allowed." });
        }

        // ۲. تولید لینک آپلود با اعتبار ۵ دقیقه
        var expiration = TimeSpan.FromMinutes(5);
        var (uploadUrl, objectKey) = await _storageService.GeneratePresignedUploadUrlAsync(
            request.FileName,
            request.ContentType,
            expiration,
            cancellationToken);

        // ۳. بازگرداندن لینک به کلاینت
        return Ok(new GenerateUploadUrlResponse 
        { 
            UploadUrl = uploadUrl, 
            ObjectKey = objectKey,
            ExpiresInSeconds = (int)expiration.TotalSeconds
        });
    }
}

public class GenerateUploadUrlRequest
{
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
}

public class GenerateUploadUrlResponse
{
    public string UploadUrl { get; set; } = string.Empty;
    public string ObjectKey { get; set; } = string.Empty;
    public int ExpiresInSeconds { get; set; }
}
