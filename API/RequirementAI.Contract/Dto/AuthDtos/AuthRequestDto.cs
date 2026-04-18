namespace RequirementAI.Contract.Dto.AuthDtos;

public class AuthRequestDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}