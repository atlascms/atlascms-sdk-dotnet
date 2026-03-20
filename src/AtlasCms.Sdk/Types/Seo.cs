using System.Text.Json.Nodes;

namespace AtlasCms.Sdk.Types;

public record ContentSeoFaq
{
    public string? Question { get; set; }
    public string? Answer { get; set; }
}

public record ContentSeoJsonld
{
    public string? Type { get; set; }
    public IReadOnlyList<ContentSeoFaq>? Faq { get; set; }
    public IReadOnlyDictionary<string, JsonNode>? AdditionalData { get; set; }
}

public record ContentSeoOpenGraph
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public string? Type { get; set; }
}

public record ContentSeoX
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public string? Card { get; set; }
}

public record ContentSeo
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Keywords { get; set; }
    public string? Slug { get; set; }
    public string? CanonicalUrl { get; set; }
    public string? Robots { get; set; }
    public ContentSeoOpenGraph? OpenGraph { get; set; }
    public ContentSeoX? X { get; set; }
    public ContentSeoJsonld? StructuredData { get; set; }
}
