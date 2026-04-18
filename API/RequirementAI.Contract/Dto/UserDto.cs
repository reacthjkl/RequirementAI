using System.ComponentModel.DataAnnotations;

namespace RequirementAI.Contract.Dto;

public class UserDto
{
    public Guid Id { get; set; }

    [Required] [MaxLength(255)] public required string Email { get; set; }

    [Required] [MaxLength(255)] public required string Name { get; set; }

    [MaxLength(500)] public string? AvatarUrl { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}