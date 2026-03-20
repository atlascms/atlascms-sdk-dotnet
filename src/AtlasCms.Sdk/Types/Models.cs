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
    public string? Label { get; set; }
    public string? Name { get; set; }
    public string? Hint { get; set; }
    public int? Order { get; set; }
    public string? Type { get; set; }
    public bool? Localizable { get; set; }
    public bool? Hidden { get; set; }
    public bool? ReadOnly { get; set; }
    public bool? Required { get; set; }
}

public record Component
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Key { get; set; }
    public string? Description { get; set; }
    public SchemaType? Type { get; set; }
    public IReadOnlyList<IField>? Attributes { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public string? ProjectId { get; set; }
}

public record Fieldset
{
    public string? Key { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? Collapsed { get; set; }
    public IReadOnlyList<string>? Fields { get; set; }
}

public record StringStringKeyValue
{
    public string? Key { get; set; }
    public string? Value { get; set; }
}

public record ModelProperties
{
    public string? Icon { get; set; }
    public IReadOnlyList<Fieldset>? Fieldsets { get; set; }
    public IReadOnlyList<StringStringKeyValue>? Links { get; set; }
}

public enum ModelFilterFieldType
{
    Boolean,
    GeoPoint,
    Date,
    DateTime,
    TimeOfTheDay,
    NumberInt,
    NumberFloat,
    String,
    StringArray,
    Json,
    Object,
    ObjectArray,
    Component,
    ComponentArray
}

public record ModelFilterModel
{
    public string? Name { get; set; }
    public IReadOnlyList<string>? Operators { get; set; }
    public string? Key { get; set; }
    public object? DefaultValue { get; set; }
    public ModelFilterFieldType? FieldType { get; set; }
}

public record Model
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Key { get; set; }
    public string? Description { get; set; }
    public SchemaType? Type { get; set; }
    public IReadOnlyList<IField>? Attributes { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public bool? EnableStageMode { get; set; }
    public bool? EnableSeo { get; set; }
    public bool? IsSingle { get; set; }
    public bool? System { get; set; }
    public bool? Localizable { get; set; }
    public ModelProperties? Properties { get; set; }
    public IReadOnlyList<ModelFilterModel>? Filters { get; set; }
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
