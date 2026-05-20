using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using RequirementAI.Business.Interfaces;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services;

public class TestService(IPersonaRepository personaRepo, IPersonaRefiner personaRefiner): ITestService
{
    public async Task TestPersonaRefinement(CancellationToken ct)
    {
        var persona = await personaRepo.GetWithProjectById(new Guid("4126512f-2e09-4d07-9d6f-971b4677d490"), ct);

        var result = await personaRefiner.Process(persona, ct);
        
        Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
        }));
    }
}