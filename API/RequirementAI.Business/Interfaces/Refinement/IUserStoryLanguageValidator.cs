using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Interfaces.Refinement;

public interface IUserStoryLanguageValidator
{
    string? GetCorrectionInstruction(
        UserStory input,
        IEnumerable<UserStoryForLLMDto> outputStories);
}
