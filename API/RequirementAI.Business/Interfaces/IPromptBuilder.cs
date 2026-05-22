using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.LLMDtos;

namespace RequirementAI.Business.Interfaces;

public interface IPromptBuilder
{
    LLMRequestDto Build<TEntity, TDto>(TEntity entity, string? customInstructions = null);
}