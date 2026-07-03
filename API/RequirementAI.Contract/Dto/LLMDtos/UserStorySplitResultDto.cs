namespace RequirementAI.Contract.Dto.LLMDtos;

public class UserStorySplitResultDto
{
    public ICollection<UserStoryForLLMDto> UserStories { get; set; } = new List<UserStoryForLLMDto>();
}
