using System.Text.Json.Nodes;

namespace AtlasCms.Sdk.Types;

/// <summary>
/// Generic representation of Atlas content where the `Attributes` payload can be either:
/// - the default untyped `JsonObject`, or
/// - a strongly-typed class (POCO) matching the attributes schema.
/// </summary>
public record Content<TAttributes>
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
    public TAttributes? Attributes { get; set; }
    public ContentSeo? Seo { get; set; }
    public IReadOnlyList<ContentLocale>? Locales { get; set; }
}

/// <summary>
/// Default (untyped) content where `Attributes` is a `JsonObject`.
/// </summary>
public record Content : Content<JsonObject>
{
}
