using RequirementAI.Contract.Enums;

namespace RequirementAI.Contract.Dto.LLMDtos;

public class LLMRequestDto(
    string prompt,
    LLMRequestPurpose purpose)
{
    public string Prompt { get; set; } = prompt;
    public LLMRequestPurpose Purpose { get; set; } = purpose;
}
