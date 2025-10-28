using Birahe.EndPoint.Constants.Enums;
using Birahe.EndPoint.Enums;
using Birahe.EndPoint.Models.ResultModels;

namespace Birahe.EndPoint.Services.Utilities;

public class MediaService {
    private readonly string _mediaRootPath;

    public MediaService(IWebHostEnvironment env) {
        // ✅ Store outside wwwroot for security
        _mediaRootPath = Path.Combine(env.ContentRootPath, "SecureFiles", "Riddles");
        if (!Directory.Exists(_mediaRootPath))
            Directory.CreateDirectory(_mediaRootPath);
    }

    // Save image and return only the filename
    public async Task<ServiceResult<(string, MediaType)>> SaveFileAsync(IFormFile? file) {
        if (file == null || file.Length == 0)
            return ServiceResult<(string, MediaType)>.Fail("فایل خالی است!");


        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        var mediaType = DetectMediaType(ext);
        if (mediaType == MediaType.Unknown)
            return ServiceResult<(string, MediaType)>.Fail("فرمت فایل نامعتبر است!");

        var mediaFolder = Path.Combine(_mediaRootPath, mediaType.ToString());
        if (!Directory.Exists(mediaFolder)) {
            Directory.CreateDirectory(mediaFolder);
        }

        var fileName = $"{Guid.NewGuid()}{ext}";
        var filePath = Path.Combine(mediaFolder, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return ServiceResult<(string, MediaType)>.Ok((fileName, mediaType));
    }

    // Read image securely by filename
    public async Task<ServiceResult<(byte[] File, string ContentType)>> ReadFileAsync(string fileName) {
        // 1️⃣ Validate input
        if (string.IsNullOrWhiteSpace(fileName))
            return ServiceResult<(byte[], string)>.Fail("نام فایل نامعتبر است!", ErrorType.Validation);

        // 2️⃣ Prevent directory traversal
        var sanitizedFileName = Path.GetFileName(fileName);
        var ext = Path.GetExtension(sanitizedFileName).ToLowerInvariant();
        var mediaType = DetectMediaType(ext);
        if (mediaType == MediaType.Unknown) {
            return ServiceResult<(byte[] File, string ContentType)>.Fail("فرمت فایل پشتیبانی نمیشود!");
        }


        var fullPath = Path.Combine(_mediaRootPath, mediaType.ToString(), sanitizedFileName);

        // 3️⃣ Check existence
        if (!File.Exists(fullPath))
            return ServiceResult<(byte[], string)>.Fail("فایل یافت نشد!", ErrorType.NotFound);

        // 4️⃣ Read file
        var bytes = await File.ReadAllBytesAsync(fullPath);

        // 5️⃣ Detect MIME type
        var contentType = GetContentType(fullPath);

        return ServiceResult<(byte[], string)>.Ok((bytes, contentType));
    }

    private MediaType DetectMediaType(string ext) => ext switch {
        ".jpg" or ".jpeg" or ".png" or ".webp" or ".gif" => MediaType.Image,
        ".mp3" or ".wav" or ".ogg" or ".m4a" or ".flac" or ".aac" => MediaType.Audio,
        _ => MediaType.Unknown
    };

    private string GetContentType(string path) {
        var ext = Path.GetExtension(path).ToLowerInvariant();
        return ext switch {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".webp" => "image/webp",
            ".gif" => "image/gif",
            ".mp3" => "audio/mpeg",
            ".wav" => "audio/wav",
            ".ogg" => "audio/ogg",
            ".m4a" => "audio/mp4",
            ".flac" => "audio/flac",
            ".aac" => "audio/aac",
            _ => "application/octet-stream"
        };
    }
}