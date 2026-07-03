using System.Text.Json;
using FluentValidation;
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
    IRefinementMerger<UserStory, UserStoryForLLMDto> merger,
    IUserStoryLanguageValidator userStoryLanguageValidator) : IUserStorySplitService
{
    private const int MaxLanguageAttempts = 2;

    public async Task<IReadOnlyList<UserStory>> SplitUserStory(
        UserStory userStory,
        string? customInstructions,
        CancellationToken ct)
    {
        var request = promptBuilder.BuildUserStorySplitPrompt(userStory, customInstructions);

        for (var attempt = 0; attempt < MaxLanguageAttempts; attempt++)
        {
            var response = await llmProvider.GetResponse(request, ct);
            var result = JsonSerializer.Deserialize<UserStorySplitResultDto>(response)
                         ?? throw new BusinessException("Response provided by LLM does not fit the user story split schema");

            await validator.ValidateAndThrowAsync(result, ct);

            var correction = userStoryLanguageValidator.GetCorrectionInstruction(userStory, result.UserStories);
            if (correction != null)
            {
                if (attempt == MaxLanguageAttempts - 1)
                    throw new BusinessException("LLM response language does not match the input user story language");

                request = WithLanguageCorrection(request, correction);
                continue;
            }

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

        throw new BusinessException("Unable to split the user story");
    }

    private static LLMRequestDto WithLanguageCorrection(LLMRequestDto request, string correction) =>
        new($"""
             {request.Prompt}

             LANGUAGE CORRECTION:
             The previous response was rejected because its language did not match the INPUT.
             {correction}
             Generate the complete JSON response again and follow this correction exactly.
             """, request.Temperature);
}
