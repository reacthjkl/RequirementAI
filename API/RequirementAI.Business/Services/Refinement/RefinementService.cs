using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RequirementAI.Business.Helpers;
using RequirementAI.Business.Interfaces;
using RequirementAI.Business.Interfaces.Refinement;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Services.Refinement;

public class RefinementService(
    IPromptBuilder promptBuilder,
    ILLMProvider llmProvider,
    IServiceProvider serviceProvider) : IRefinementService
{
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

        var refined = await LLMResponseRetryHelper.GetValidatedResponse<TDto>(
            llmProvider,
            request,
            (result, token) => Validate(result, token),
            "Response provided by LLM does not fit to the object schema",
            ct);

        var merger = serviceProvider.GetRequiredService<IRefinementMerger<TEntity, TDto>>();
        merger.Apply(entity, refined);
        return entity;
    }

    private async Task Validate<TDto>(TDto item, CancellationToken ct)
    {
        var validator = serviceProvider.GetService(typeof(IValidator<TDto>)) as IValidator<TDto>
                        ?? throw new BusinessException($"Validator for {typeof(TDto).Name} has not been provided");

        await validator.ValidateAndThrowAsync(item, ct);
    }
}
