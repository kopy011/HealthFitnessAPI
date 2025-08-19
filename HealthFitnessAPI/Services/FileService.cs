using HealthFitnessAPI.Model.Dtos;

namespace HealthFitnessAPI.Services;

public interface IFileService
{
    Task<string> SaveBase64PngAsync(string base64, string? fileName = null, CancellationToken ct = default);


    Task DeleteFileAsync(string fileName, CancellationToken ct = default);

    Task<IReadOnlyList<FileDto>> GetAllFilesAsync(CancellationToken ct = default);

    Task<FileDto> GetFileAsync(string fileName, CancellationToken ct = default);
}

public class FileService : IFileService
{
    private static readonly byte[] PngMagic = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
    private readonly string _badgesDir;

    public FileService()
    {
        var docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        _badgesDir = Path.Combine(docs, "Badges");
        Directory.CreateDirectory(_badgesDir);
    }

    public async Task<string> SaveBase64PngAsync(string base64, string? fileName = null, CancellationToken ct = default)
    {
        var bytes = ParseBase64Png(base64);

        var safeName = SanitizeFileName(fileName);
        if (string.IsNullOrWhiteSpace(safeName))
            safeName = $"{Guid.NewGuid():N}.png";
        if (!safeName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            safeName += ".png";

        var targetPath = Path.Combine(_badgesDir, safeName);
        if (File.Exists(targetPath))
        {
            var name = Path.GetFileNameWithoutExtension(safeName);
            var ext = Path.GetExtension(safeName);
            safeName = $"{name}-{Guid.NewGuid():N}{ext}";
            targetPath = Path.Combine(_badgesDir, safeName);
        }

        await File.WriteAllBytesAsync(targetPath, bytes, ct);
        return safeName;
    }

    public Task DeleteFileAsync(string fileName, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("Filename is required.", nameof(fileName));

        var safeName = EnsurePngExtension(SanitizeFileName(fileName));
        var path = Path.Combine(_badgesDir, safeName);

        if (!File.Exists(path))
            throw new FileNotFoundException("File not found.", safeName);

        File.Delete(path);
        return Task.CompletedTask;
    }

    public async Task<IReadOnlyList<FileDto>> GetAllFilesAsync(CancellationToken ct = default)
    {
        if (!Directory.Exists(_badgesDir))
            return Array.Empty<FileDto>();

        var files = Directory.GetFiles(_badgesDir, "*.png", SearchOption.TopDirectoryOnly);
        var list = new List<FileDto>(files.Length);

        foreach (var path in files)
        {
            ct.ThrowIfCancellationRequested();
            var bytes = await File.ReadAllBytesAsync(path, ct);
            var base64 = Convert.ToBase64String(bytes);
            var dto = new FileDto(Path.GetFileName(path), $"data:image/png;base64,{base64}");
            list.Add(dto);
        }

        return list;
    }

    public async Task<FileDto> GetFileAsync(string fileName, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("Filename is required.", nameof(fileName));

        var safeName = EnsurePngExtension(SanitizeFileName(fileName));
        var path = Path.Combine(_badgesDir, safeName);

        if (!File.Exists(path))
            throw new FileNotFoundException("File not found.", safeName);

        var bytes = await File.ReadAllBytesAsync(path, ct);
        var base64 = Convert.ToBase64String(bytes);

        return new FileDto(safeName, $"data:image/png;base64,{base64}");
    }

    private static string SanitizeFileName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return string.Empty;
        var justName = Path.GetFileName(name);
        foreach (var c in Path.GetInvalidFileNameChars())
            justName = justName.Replace(c, '_');
        return justName.Trim();
    }

    private static string EnsurePngExtension(string fileName)
    {
        return fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ? fileName : fileName + ".png";
    }

    private static byte[] ParseBase64Png(string base64)
    {
        if (string.IsNullOrWhiteSpace(base64))
            throw new ArgumentException("Base64 content is required.", nameof(base64));

        var idx = base64.IndexOf("base64,", StringComparison.OrdinalIgnoreCase);
        if (idx >= 0) base64 = base64[(idx + "base64,".Length)..];

        byte[] bytes;
        try
        {
            bytes = Convert.FromBase64String(base64);
        }
        catch (FormatException ex)
        {
            throw new ArgumentException("Invalid Base64 content.", nameof(base64), ex);
        }

        if (bytes.Length < PngMagic.Length || !PngMagic.SequenceEqual(bytes.Take(PngMagic.Length)))
            throw new InvalidDataException("Content is not a valid PNG (bad signature).");

        return bytes;
    }
}