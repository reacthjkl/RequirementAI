using RequirementAI.Contract.Enums;
using RequirementAI.Contract.Settings;

namespace RequirementAI.Business.Interfaces;

public interface ILLMRouteResolver
{
    ResolvedLLMRoute Resolve(LLMRequestPurpose purpose);
}

public record ResolvedLLMRoute(
    string ProviderId,
    LLMProviderSettings Provider,
    string Model)
{
    public string Identifier => $"{ProviderId}/{Model}";
}
