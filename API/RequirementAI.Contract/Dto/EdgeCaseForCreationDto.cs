namespace RequirementAI.Contract.Dto;

public class EdgeCaseForCreationDto
{    
    public string Preconditions { get; set; } = null!;
    public string TriggerAction { get; set; } = null!;
    public string ExpectedBehavior { get; set; } = null!;
    public Guid UserStoryId { get; set; }
}