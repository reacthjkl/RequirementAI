using RequirementAI.Contract.Enums;

namespace RequirementAI.Contract.Dto.AuthDtos;

public class ExternalAuthRequestDto
{
    public required AuthProvider Provider { get; set; }

    public required string Token { get; set; }
}