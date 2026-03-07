using System.Text.Json;

namespace AtlasCms.Sdk.Types;

public record Media
{
    public required string Id { get; set; }
    public required string Code { get; set; }
    public required string Folder { get; set; }
    public required string Type { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public required string CreatedBy { get; set; }
    public required DateTimeOffset ModifiedAt { get; set; }
    public required string ModifiedBy { get; set; }
    public string? Author { get; set; }
    public string? Copyright { get; set; }
    public required string OriginalFileName { get; set; }
    public required string Name { get; set; }
    public required string Format { get; set; }
    public required string Hash { get; set; }
    public required string MimeType { get; set; }
    public long Size { get; set; }
    public IReadOnlyList<string> AutomaticTags { get; set; } = [];
    public IReadOnlyList<string> Tags { get; set; } = [];
    public required string Url { get; set; }
    public required string Provider { get; set; }
    public int? Height { get; set; }
    public int? Width { get; set; }
    public double? HorizontalResolution { get; set; }
    public double? VerticalResolution { get; set; }
    public double? Duration { get; set; }
    public double? Fps { get; set; }
    public string? Codec { get; set; }
    public JsonElement? Exif { get; set; }
}
