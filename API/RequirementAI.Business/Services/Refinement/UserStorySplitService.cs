using FluentValidation;
using RequirementAI.Business.Helpers;
using RequirementAI.Business.Interfaces;
using RequirementAI.Business.Interfaces.Refinement;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Contract.Enums;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Services.Refinement;

public class UserStorySplitService(
    IPromptBuilder promptBuilder,
    ILLMProvider llmProvider,
    IValidator<UserStorySplitResultDto> validator,
    IRefinementMerger<UserStory, UserStoryForLLMDto> merger) : IUserStorySplitService
{
    public async Task<IReadOnlyList<UserStory>> SplitUserStory(
        UserStory userStory,
        string? customInstructions,
        CancellationToken ct)
    {
        var request = promptBuilder.BuildUserStorySplitPrompt(userStory, customInstructions);

        var result = await LLMResponseRetryHelper.GetValidatedResponse<UserStorySplitResultDto>(
            llmProvider,
            request,
            (result, token) => validator.ValidateAndThrowAsync(result, token),
            "Response provided by LLM does not fit the user story split schema",
            ct);

        merger.Apply(userStory, result.UserStories.First());

        var splitStories = new List<UserStory> { userStory };
        foreach (var splitDto in result.UserStories.Skip(1))
        {
            var splitStory = new UserStory
            {
                ScenarioId = userStory.ScenarioId,
                Scenario = userStory.Scenario,
                Stage = UserStoryStage.New
            };

            merger.Apply(splitStory, splitDto);
            splitStories.Add(splitStory);
        }

        return splitStories;
    }
}
