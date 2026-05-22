namespace RequirementAI.Contract.Dto;

public class EdgeCaseForUpdateDto
{
    public Guid Id { get; set; }
    public string Preconditions { get; set; } = null!;
    public string TriggerAction { get; set; } = null!;
    public string ExpectedBehavior { get; set; } = null!;
}