using System.Text.Json;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RequirementAI.Business.Interfaces;
using RequirementAI.Business.Interfaces.QualityAnalysis;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Services.QualityAnalysis;

public class QualityAnalysisService(IPromptBuilder promptBuilder, ILLMProvider llmProvider, IServiceProvider serviceProvider): IQualityAnalysisService
{
    public async Task<Persona> AnalyzePersona(Persona persona, CancellationToken ct)
    {
        return await Analyze<Persona, PersonaForLLMDto, PersonaLlmAnalysisDto>(persona, ct);
    }

    public async Task<Scenario> AnalyzeScenario(Scenario scenario, CancellationToken ct)
    {
        return await Analyze<Scenario, ScenarioForLLMDto, ScenarioLlmAnalysisDto>(scenario, ct);
    }

    public async Task<UserStory> AnalyzeUserStory(UserStory userStory, CancellationToken ct)
    {
        return await Analyze<UserStory, UserStoryForLLMDto, UserStoryLlmAnalysisDto>(userStory, ct);
    }

    private async Task<TEntity> Analyze<TEntity, TRequestDto, TResponseDto>(TEntity entity, CancellationToken ct)
    {
        var request = promptBuilder.BuildAnalysisPrompt<TEntity, TRequestDto, TResponseDto>(entity);
        
        var response = await llmProvider.GetResponse(
            request,
            ct);
        
        var refined = JsonSerializer.Deserialize<TResponseDto>(response) 
                      ?? throw new BusinessException("Response provided by LLM does not fit to the object schema");

        await Validate(refined, ct);
        
        var merger = serviceProvider.GetRequiredService<IAnalysisMerger<TEntity, TResponseDto>>();
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