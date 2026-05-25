namespace RequirementAI.Contract.Dto.LLMDtos;

public abstract class AnalysisBaseDto
{
    public string Strengths { get; set; } = null!;
    public string Weaknesses { get; set; } = null!;
    public string Suggestions { get; set; } = null!;
}