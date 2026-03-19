using System.Text.Json.Nodes;

namespace AtlasCms.Sdk.Types;

public enum SchemaType
{
    Model,
    Component,
    User
}

public record IField
{
    public string? Key { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public bool IsRequired { get; set; }
    public bool IsMultiple { get; set; }
    public object? DefaultValue { get; set; }
    public IReadOnlyDictionary<string, object>? Validations { get; set; }
    public string? HelpText { get; set; }
}

public record Component
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Key { get; set; }
    public string? Description { get; set; }
    public required SchemaType Type { get; set; }
    public IReadOnlyList<IField>? Attributes { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public string? ProjectId { get; set; }
}

public record ComponentModel
{
    public string? Id { get; set; }
    public string? ModelKey { get; set; }
    public string? Locale { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public string? Hash { get; set; }
    public string? Status { get; set; }
    public ContentSeo? Seo { get; set; }
    public JsonObject? Attributes { get; set; }
    public IReadOnlyList<LocaleStatus>? Locales { get; set; }
}
