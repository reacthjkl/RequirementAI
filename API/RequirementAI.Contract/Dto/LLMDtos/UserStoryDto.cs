using System.Text.Json.Serialization;

namespace RequirementAI.Contract.Dto.LLMDtos;

public class UserStoryDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<string> AcceptanceCriteria { get; set; } = new List<string>();
}