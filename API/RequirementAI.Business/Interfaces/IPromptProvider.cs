using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces;

public interface IPromptProvider
{
    LLMRequestDto Build<T>(string input);
}