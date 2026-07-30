
namespace RequirementAI.Persistence.Entities;

public class User: BaseEntity
{
    public required string Email { get; set; }
    public string? Password { get; set; }
    public required string Name { get; set; }
    public string? RefreshToken { get; set; }
    public DateTimeOffset? RefreshTokenExpiry { get; set; }
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;
}
