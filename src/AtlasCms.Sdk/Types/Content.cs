using System.Text.Json.Nodes;

namespace AtlasCms.Sdk.Types;

public record Content
{
    public required string Id { get; set; }
    public required string ModelKey { get; set; }
    public required string Locale { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public required string CreatedBy { get; set; }
    public required DateTimeOffset ModifiedAt { get; set; }
    public required string ModifiedBy { get; set; }
    public required string Hash { get; set; }
    public required ContentStatus Status { get; set; }
    public JsonObject? Attributes { get; set; }
    public ContentSeo? Seo { get; set; }
    public IReadOnlyList<LocaleStatus>? Locales { get; set; }
}
