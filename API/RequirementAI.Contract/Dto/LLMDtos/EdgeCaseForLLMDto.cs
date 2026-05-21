namespace RequirementAI.Contract.Dto.LLMDtos;

public class EdgeCaseForLLMDto
{
    public string Preconditions { get; set; } = string.Empty;
    public string TriggerAction { get; set; } = string.Empty;
    public string ExpectedBehavior { get; set; } = string.Empty;
}