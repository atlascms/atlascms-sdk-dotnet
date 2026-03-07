using System.Text.Json.Serialization;

namespace AtlasCms.Sdk.Types;

#if NET9_0_OR_GREATER
[JsonConverter(typeof(JsonStringEnumConverter<ContentStatus>))]
#else
[JsonConverter(typeof(JsonStringEnumConverter))]
#endif
public enum ContentStatus
{
#if NET9_0_OR_GREATER
    [JsonStringEnumMemberName("published")]
    Published,

    [JsonStringEnumMemberName("unpublished")]
    Unpublished
#else
    Published,
    Unpublished
#endif
}

public record LocaleStatus
{
    public required string Locale { get; set; }
    public required ContentStatus Status { get; set; }
}
