namespace RequirementAI.Contract.Dto;

public class ProjectForUpdateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}