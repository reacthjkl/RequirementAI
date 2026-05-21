namespace RequirementAI.Contract.Dto;

public class OrganizationResponseDto
{
    public Guid Id  { get; set; }
    public string Name { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
}