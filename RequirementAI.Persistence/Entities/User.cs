using System.ComponentModel.DataAnnotations;
using RequirementAI.Contract.Enums;

namespace RequirementAI.Persistence.Entities;

public class User
{
    public Guid Id { get; set; }

    [Required] [MaxLength(255)] public required string Email { get; set; }

    [MaxLength(255)] public string? Password { get; set; }

    [Required] [MaxLength(255)] public required string FirstName { get; set; }

    [Required] [MaxLength(255)] public required string LastName { get; set; }

    [Required] public AuthProvider Provider { get; set; }

    [MaxLength(255)] public string? ProviderId { get; set; }

    [MaxLength(500)] public string? AvatarUrl { get; set; }

    [MaxLength(255)] public string? RefreshToken { get; set; }

    public DateTimeOffset? RefreshTokenExpiry { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public bool EmailConfirmed { get; set; } = false;

    public Guid? EmailConfirmationToken { get; set; }

    public DateTimeOffset? EmailConfirmationTokenExpiry { get; set; }
}