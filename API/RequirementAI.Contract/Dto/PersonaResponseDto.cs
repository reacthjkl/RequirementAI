namespace RequirementAI.Contract.Dto;

public class PersonaResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ContextOfUse { get; set; } = null!;
    public string Goals { get; set; } = null!;
    public string Frustrations { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public Guid ProjectId { get; set; }
}