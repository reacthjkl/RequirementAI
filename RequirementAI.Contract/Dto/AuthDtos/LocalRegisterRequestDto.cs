namespace RequirementAI.Contract.Dto.AuthDtos;

public class LocalRegisterRequestDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Name { get; set; }
}