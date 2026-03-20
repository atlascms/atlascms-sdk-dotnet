namespace AtlasCms.Sdk.Types;

public enum ContentStatus
{
    Published,
    Unpublished
}

public record LocaleStatus
{
    public required string Locale { get; set; }
    public required ContentStatus Status { get; set; }
}

public record ContentLocale
{
    public string? Locale { get; set; }
    public string? Id { get; set; }
}
