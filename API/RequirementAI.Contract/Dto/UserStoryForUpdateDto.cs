namespace RequirementAI.Contract.Dto;

public class UserStoryForUpdateDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
}