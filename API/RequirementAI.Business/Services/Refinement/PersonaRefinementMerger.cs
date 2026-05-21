using AutoMapper;
using RequirementAI.Business.Interfaces;
using RequirementAI.Business.Interfaces.Refinement;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Services.Refinement;

public class PersonaRefinementMerger(IMapper mapper): IRefinementMerger<Persona, PersonaForLLMDto>
{
    public void Apply(Persona entity, PersonaForLLMDto dto)
    {
        mapper.Map(dto, entity);
    }
}