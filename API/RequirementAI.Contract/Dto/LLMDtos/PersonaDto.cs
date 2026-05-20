namespace RequirementAI.Contract.Dto.LLMDtos;

public class PersonaDto
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    
    public string ContextOfUse { get; set; } = string.Empty;

    public string Goals { get; set; } = string.Empty;
    
    public string Frustrations { get; set; } = string.Empty;
}