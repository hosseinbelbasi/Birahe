using Birahe.EndPoint.Enums;
using Birahe.EndPoint.Models.ResultModels;

namespace Birahe.EndPoint.Services.Utilities;

public class ImageService
{
    private readonly string _imageRootPath;

    public ImageService(IWebHostEnvironment env)
    {
        // ✅ Store outside wwwroot for security
        _imageRootPath = Path.Combine(env.ContentRootPath, "SecureImages", "Riddles");
        if (!Directory.Exists(_imageRootPath))
            Directory.CreateDirectory(_imageRootPath);
    }

    // Save image and return only the filename
    public async Task<ServiceResult<string>> SaveImageAsync(IFormFile? image)
    {
        if (image == null || image.Length == 0)
            return ServiceResult<string>.Fail("عکس خالی است!");

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var ext = Path.GetExtension(image.FileName).ToLower();
        if (!allowedExtensions.Contains(ext))
            return ServiceResult<string>.Fail("فرمت عکس نامعتبر است!");

        var fileName = $"{Guid.NewGuid()}{ext}";
        var filePath = Path.Combine(_imageRootPath, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await image.CopyToAsync(stream);

        // ✅ Return only the filename for DB storage
        return ServiceResult<string>.Ok(fileName);
    }

    // Read image securely by filename
    public async Task<ServiceResult<(byte[] File, string ContentType)>> ReadImageAsync(string fileName)
    {
        // 1️⃣ Validate input
        if (string.IsNullOrWhiteSpace(fileName))
            return ServiceResult<(byte[], string)>.Fail("نام فایل نامعتبر است!", ErrorType.Validation);

        // 2️⃣ Prevent directory traversal
        var sanitizedFileName = Path.GetFileName(fileName);
        var fullPath = Path.Combine(_imageRootPath, sanitizedFileName);

        // 3️⃣ Check existence
        if (!File.Exists(fullPath))
            return ServiceResult<(byte[], string)>.Fail("فایل تصویر یافت نشد!", ErrorType.NotFound);

        // 4️⃣ Read file
        var bytes = await File.ReadAllBytesAsync(fullPath);

        // 5️⃣ Detect MIME type
        var contentType = GetContentType(fullPath);

        return ServiceResult<(byte[], string)>.Ok((bytes, contentType));
    }

    private string GetContentType(string path)
    {
        var ext = Path.GetExtension(path).ToLowerInvariant();
        return ext switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".webp" => "image/webp",
            ".gif" => "image/gif",
            _ => "application/octet-stream"
        };
    }
}
