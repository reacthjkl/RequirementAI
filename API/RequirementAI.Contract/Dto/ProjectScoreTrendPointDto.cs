namespace RequirementAI.Contract.Dto;

public class ProjectScoreTrendPointDto
{
    public DateTimeOffset Date { get; set; }

    public double Score { get; set; }

    public string? Label { get; set; } 
}