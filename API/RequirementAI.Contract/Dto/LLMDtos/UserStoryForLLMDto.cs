
namespace RequirementAI.Contract.Dto.LLMDtos;

public class UserStoryForLLMDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<AcceptanceCriteriaForLLMDto> AcceptanceCriteria { get; set; } = new List<AcceptanceCriteriaForLLMDto>();
    public ICollection<EdgeCaseForLLMDto> EdgeCases { get; set; } = new List<EdgeCaseForLLMDto>();
}