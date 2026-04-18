namespace RequirementAI.Contract.Dto;

public class LLMRequestDto(string prompt, double temperature = 2.0)
{
    public string Prompt { get; set; } = prompt;
    public double Temperature { get; set; } = temperature;
}