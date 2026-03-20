namespace AtlasCms.Sdk.Types;

public record Role
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public bool System { get; set; }
    public IReadOnlyList<string>? Permissions { get; set; }
}

public record PermissionSection
{
    public string? Name { get; set; }
    public string? Feature { get; set; }
    public IReadOnlyList<string>? Permissions { get; set; }
}

public record PermissionGroup
{
    public string? Group { get; set; }
    public string? Type { get; set; }
    public string? Key { get; set; }
    public IReadOnlyList<PermissionSection>? Sections { get; set; }
}
