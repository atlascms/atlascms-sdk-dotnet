using System.Text.Json;

namespace AtlasCms.Sdk.Types;

public record Media
{
    public string? Id { get; set; }
    public string? Code { get; set; }
    public string? Folder { get; set; }
    public string? Type { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public string? Author { get; set; }
    public string? Copyright { get; set; }
    public string? OriginalFileName { get; set; }
    public string? Name { get; set; }
    public string? Format { get; set; }
    public string? Hash { get; set; }
    public string? MimeType { get; set; }
    public long Size { get; set; }
    public IReadOnlyList<string>? AutomaticTags { get; set; }
    public IReadOnlyList<string>? Tags { get; set; }
    public string? Url { get; set; }
    public string? Provider { get; set; }
    public int? Height { get; set; }
    public int? Width { get; set; }
    public double? HorizontalResolution { get; set; }
    public double? VerticalResolution { get; set; }
    public double? Duration { get; set; }
    public double? Fps { get; set; }
    public string? Codec { get; set; }
    public JsonElement? Exif { get; set; }
}
