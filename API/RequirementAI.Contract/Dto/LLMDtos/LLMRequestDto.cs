namespace RequirementAI.Contract.Dto.LLMDtos;

public class LLMRequestDto(string prompt, float temperature = 2f)
{
    public string Prompt { get; set; } = prompt;
    public float Temperature { get; set; } = temperature;
}