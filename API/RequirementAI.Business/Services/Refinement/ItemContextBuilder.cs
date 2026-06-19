using System.Text.Encodings.Web;
using System.Text.Json;
using AutoMapper;
using RequirementAI.Business.Interfaces.Refinement;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Services.Refinement;

public class ItemContextBuilder(IMapper mapper): IItemContextBuilder
{
    public string Build<T>(T entity)
    {
        return entity switch
        {
            Persona persona => BuildPersonaContext(persona),
            Scenario scenario => BuildScenarioContext(scenario),
            UserStory userStory => BuildUserStoryContext(userStory),
            _ => throw new NotSupportedException(
                $"Refinement context is not supported for type {typeof(T).Name}")
        };    }
    
    private string BuildPersonaContext(Persona persona)
    {
        return $"""
                Project description:
                {persona.Project.Description}

                Other personas:
                {
                    JsonSerializer.Serialize(
                        mapper.Map<List<PersonaForLLMDto>>(
                            persona.Project.Personas.Where(p => p.Id != persona.Id)), new JsonSerializerOptions
                        {
                            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                        })
                }
                """;
    }

    private string BuildScenarioContext(Scenario scenario)
    {
        return $"""
                Project description:
                {scenario.Persona.Project.Description}

                Persona for this scenario:
                {JsonSerializer.Serialize(mapper.Map<PersonaForLLMDto>(scenario.Persona), new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                })}

                Other scenarios:
                {
                    JsonSerializer.Serialize(
                        mapper.Map<List<ScenarioForLLMDto>>(
                            scenario.Persona.Scenarios.Where(p => p.Id != scenario.Id)), new JsonSerializerOptions
                        {
                            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                        })
                }
                """;
    }

    private string BuildUserStoryContext(UserStory userStory)
    {
        return $"""
            Project description:
            {userStory.Scenario.Persona.Project.Description}

            Scenario for this user story:
            {JsonSerializer.Serialize(mapper.Map<ScenarioForLLMDto>(userStory.Scenario), new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            })}

            Persona for this user story:
            {JsonSerializer.Serialize(mapper.Map<PersonaForLLMDto>(userStory.Scenario.Persona), new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            })}

            Other user stories:
            {
                JsonSerializer.Serialize(
                    mapper.Map<List<UserStoryForLLMDto>>(
                        userStory.Scenario.UserStories.Where(us => us.Id != userStory.Id)), new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    })
            }
            """;
    }
}