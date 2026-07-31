using System.Text.Json.Serialization;

namespace RequirementAI.Contract.Dto.LLMDtos;

[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public class ScenarioLlmAnalysisDto: AnalysisBaseDto
{
    public int ClarityScore { get; set; }
    public int ContextScore { get; set; }
    public int TriggerScore { get; set; }
    public int FlowCompletenessScore { get; set; }
    public int EdgeCasesScore { get; set; }
    public int PersonaFitScore { get; set; }
}
