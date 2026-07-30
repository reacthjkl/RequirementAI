namespace RequirementAI.Contract.Dto;

public class ProjectWordCountDto
{
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; } = null!;
    public int TotalWords { get; set; }
    public decimal AverageWordsPerPersona { get; set; }
    public decimal AverageWordsPerScenario { get; set; }
    public decimal AverageWordsPerUserStory { get; set; }
    public List<ArtifactWordCountDto> WordsPerPersona { get; set; } = [];
    public List<ArtifactWordCountDto> WordsPerScenario { get; set; } = [];
    public List<ArtifactWordCountDto> WordsPerUserStory { get; set; } = [];
}

public class ArtifactWordCountDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public int Words { get; set; }
}
