using System.Text.Json;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RequirementAI.Business.Interfaces;
using RequirementAI.Business.Interfaces.Refinement;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Services.Refinement;

public class RefinementService(
    IPromptBuilder promptBuilder, 
    ILLMProvider llmProvider, 
    IServiceProvider serviceProvider,
    IUserStoryLanguageValidator userStoryLanguageValidator): IRefinementService
{
    private const int MaxLanguageAttempts = 2;

    public async Task<Persona> RefinePersona(Persona persona, string? customInstructions, CancellationToken ct)
    {
        return await Refine<Persona, PersonaForLLMDto>(persona, customInstructions, ct);
    }
    
    public async Task<Scenario> RefineScenario(Scenario scenario, string? customInstructions, CancellationToken ct)
    {
        return await Refine<Scenario, ScenarioForLLMDto>(scenario, customInstructions, ct);
    }

    public async Task<UserStory> RefineUserStory(UserStory userStory, string? customInstructions,
        CancellationToken ct)
    {
        return await Refine<UserStory, UserStoryForLLMDto>(userStory, customInstructions, ct);
    }
    
    private async Task<TEntity> Refine<TEntity, TDto>(TEntity entity,
        string? customInstructions,
        CancellationToken ct)
    {
        var request = promptBuilder.BuildRefinementPrompt<TEntity, TDto>(entity, customInstructions);

        for (var attempt = 0; attempt < MaxLanguageAttempts; attempt++)
        {
            var response = await llmProvider.GetResponse(request, ct);
            var refined = JsonSerializer.Deserialize<TDto>(response)
                          ?? throw new BusinessException("Response provided by LLM does not fit to the object schema");

            await Validate(refined, ct);

            if (entity is UserStory inputStory && refined is UserStoryForLLMDto outputStory)
            {
                var correction = userStoryLanguageValidator.GetCorrectionInstruction(inputStory, [outputStory]);
                if (correction != null)
                {
                    if (attempt == MaxLanguageAttempts - 1)
                        throw new BusinessException("LLM response language does not match the input user story language");

                    request = WithLanguageCorrection(request, correction);
                    continue;
                }
            }

            var merger = serviceProvider.GetRequiredService<IRefinementMerger<TEntity, TDto>>();
            merger.Apply(entity, refined);
            return entity;
        }

        throw new BusinessException("Unable to refine the entity");
    }

    private static LLMRequestDto WithLanguageCorrection(LLMRequestDto request, string correction) =>
        new($"""
             {request.Prompt}

             LANGUAGE CORRECTION:
             The previous response was rejected because its language did not match the INPUT.
             {correction}
             Generate the complete JSON response again and follow this correction exactly.
             """, request.Purpose, request.Temperature);

    private async Task Validate<TDto>(TDto item, CancellationToken ct)
    {
        var validator = serviceProvider.GetService(typeof(IValidator<TDto>)) as IValidator<TDto>
                        ?? throw new BusinessException($"Validator for {typeof(TDto).Name} has not been provided");

        await validator.ValidateAndThrowAsync(item, ct);
    }
}
