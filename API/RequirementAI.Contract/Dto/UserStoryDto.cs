using System.Text.Json.Serialization;

namespace RequirementAI.Contract.Dto;

public class UserStoryDto
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("acceptanceCriteria")]
    public ICollection<string> AcceptanceCriteria { get; set; } = new List<string>();

}