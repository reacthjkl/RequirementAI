namespace RequirementAI.Contract.Dto.AuthDtos;

public class UserIdentityPayload
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string ProviderId { get; set; }
    public required string AvatarUrl { get; set; }
}