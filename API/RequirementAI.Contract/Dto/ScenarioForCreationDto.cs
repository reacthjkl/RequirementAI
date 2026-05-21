namespace RequirementAI.Contract.Dto;

public class ScenarioForCreationDto
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public Guid PersonaId { get; set; }
}