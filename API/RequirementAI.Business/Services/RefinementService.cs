using System.Text.Json;
using AutoMapper;
using FluentValidation;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Services;

public class RefinementService(
    IPromptBuilder promptBuilder, 
    ILLMProvider llmProvider, 
    IMapper mapper,
    IServiceProvider serviceProvider): IRefinementService
{
    public async Task<Persona> RefinePersona(Persona persona, CancellationToken ct)
    {
        var context = persona.Project.Description;

        return await Refine<Persona, PersonaForLLMDto>(persona, context, ct);
    }
    
    public async Task<Scenario> RefineScenario(Scenario scenario, CancellationToken ct)
    {
        var context = $"""
                      Project description:
                      {scenario.Persona.Project.Description}
                      
                      Persona for this scenario:
                      {JsonSerializer.Serialize(mapper.Map<PersonaForLLMDto>(scenario.Persona))}
                      
                      Other personas:
                      {JsonSerializer.Serialize(mapper.Map<List<PersonaForLLMDto>>(scenario.Persona.Project.Personas))}
                      """;
        
        return await Refine<Scenario, ScenarioForLLMDto>(scenario, context, ct);
    }

    public async Task<UserStory> RefineUserStory(UserStory userStory, CancellationToken ct)
    {
        var context = $"""
                       Project description:
                       {userStory.Scenario.Persona.Project.Description}
                       
                       Scenario for this user story:
                       {JsonSerializer.Serialize(mapper.Map<ScenarioForLLMDto>(userStory.Scenario))}

                       Persona for this user story:
                       {JsonSerializer.Serialize(mapper.Map<PersonaForLLMDto>(userStory.Scenario.Persona))}

                       Other user stories:
                       {JsonSerializer.Serialize(mapper.Map<List<UserStoryForLLMDto>>(userStory.Scenario.UserStories))}
                       """;
        
        return await Refine<UserStory, UserStoryForLLMDto>(userStory, context, ct);
    }
    
    private async Task<TEntity> Refine<TEntity, TDto>(
        TEntity entity,
        string context,
        CancellationToken ct)
    {
        var input = JsonSerializer.Serialize(mapper.Map<TDto>(entity));

        var response = await llmProvider.GetResponse(
            promptBuilder.Build<TDto>(input, context),
            ct);

        var refined = JsonSerializer.Deserialize<TDto>(response) 
                      ?? throw new BusinessException("Response provided by LLM does not fit to the object schema");

        await Validate(refined, ct);
        
        mapper.Map(refined, entity);

        return entity;
    }

    private async Task Validate<TDto>(TDto item, CancellationToken ct)
    {
        var validator = serviceProvider.GetService(typeof(IValidator<TDto>)) as IValidator<TDto>
                        ?? throw new BusinessException($"Validator for {typeof(TDto).Name} has not been provided");

        await validator.ValidateAsync(item, ct);
    }
}