using System.Text.Json.Nodes;

namespace AtlasCms.Sdk.Types;

public record Content
{
    public string? Id { get; set; }
    public string? ModelKey { get; set; }
    public string? Locale { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public string? Hash { get; set; }
    public ContentStatus? Status { get; set; }
    public JsonObject? Attributes { get; set; }
    public ContentSeo? Seo { get; set; }
    public IReadOnlyList<ContentLocale>? Locales { get; set; }
}
