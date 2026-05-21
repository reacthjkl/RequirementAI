namespace RequirementAI.Contract.Dto;

public class ScenarioForUpdateDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
}