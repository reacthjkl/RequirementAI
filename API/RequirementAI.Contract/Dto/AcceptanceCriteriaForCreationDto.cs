namespace RequirementAI.Contract.Dto;

public class AcceptanceCriteriaForCreationDto
{
    public string Wording { get; set; } = null!;
    public Guid UserStoryId { get; set; }
}