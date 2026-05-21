namespace RequirementAI.Contract.Dto;

public class ProjectForCreationDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid OrganizationId { get; set; }
}