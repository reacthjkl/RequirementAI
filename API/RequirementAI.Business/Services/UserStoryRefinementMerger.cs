using AutoMapper;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Services;

public class UserStoryRefinementMerger(IMapper mapper): IRefinementMerger<UserStory, UserStoryForLLMDto>
{
    public void Apply(UserStory entity, UserStoryForLLMDto dto)
    {
        mapper.Map(dto, entity);

        // completely replace acceptance criteria entities with new ones
        entity.AcceptanceCriteria.Clear();
        
        foreach (var ac in dto.AcceptanceCriteria)
        {
            entity.AcceptanceCriteria.Add(
                mapper.Map<AcceptanceCriteria>(ac)
            );
        }

        // completely replace edge case entities with new ones
        entity.EdgeCases.Clear();
        
        foreach (var ec in dto.EdgeCases)
        {
            entity.EdgeCases.Add(
                mapper.Map<EdgeCase>(ec)
            );
        }
        
    }
}