namespace RequirementAI.Contract.Dto;

public class ProjectQualityCriterionAverageDto
{
    public string ArtifactType { get; set; } = null!;
    public string CriterionName { get; set; } = null!;
    public decimal AverageScore { get; set; }
}
