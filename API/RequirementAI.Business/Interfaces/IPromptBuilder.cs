using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Interfaces;

public interface IPromptBuilder
{
    LLMRequestDto BuildRefinementPrompt<TEntity, TDto>(TEntity entity, string? customInstructions = null);
    LLMRequestDto BuildUserStorySplitPrompt(UserStory userStory, string? customInstructions = null);
    LLMRequestDto BuildAnalysisPrompt<TEntity, TRequestDto, TResponseDto>(TEntity entity);
}
