using System.Text.Json;
using AutoMapper;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Services;

public class PersonaRefiner(IPromptProvider promptProvider, ILLMProvider llmProvider, IMapper mapper): IPersonaRefiner
{
    public async Task<Persona> Process(Persona persona, CancellationToken ct)
    {
        var mapped = mapper.Map<PersonaDto>(persona);
        
        var input = JsonSerializer.Serialize(mapped);
        var context = persona.Project.Description;
        
        var request = promptProvider.Build<PersonaDto>(input, context);

        var response = await llmProvider.GetResponse(request, ct);

        var refined = JsonSerializer.Deserialize<PersonaDto>(response)
            ?? throw new BusinessException("Response provided by LLM does not fit to the object schema");
        
        // todo: validate output
        
        mapper.Map(refined, persona);

        return persona;
    }
}