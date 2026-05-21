namespace RequirementAI.Contract.Dto;

public class ScenarioResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public Guid PersonaId { get; set; }
}