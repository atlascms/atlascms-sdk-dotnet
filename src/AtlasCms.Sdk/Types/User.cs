using System.Text.Json.Nodes;

namespace AtlasCms.Sdk.Types;

public record User
{
    public string? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? MobilePhone { get; set; }
    public IReadOnlyList<string>? Roles { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public string? Notes { get; set; }
    public string? Picture { get; set; }
    public JsonObject? Attributes { get; set; }
}
