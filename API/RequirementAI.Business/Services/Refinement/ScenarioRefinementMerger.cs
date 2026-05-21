using AutoMapper;
using RequirementAI.Business.Interfaces;
using RequirementAI.Business.Interfaces.Refinement;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Services.Refinement;

public class ScenarioRefinementMerger(IMapper mapper): IRefinementMerger<Scenario, ScenarioForLLMDto>
{
    public void Apply(Scenario entity, ScenarioForLLMDto dto)
    {
        mapper.Map(dto, entity);
    }
}