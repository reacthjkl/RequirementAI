using Microsoft.Extensions.Configuration;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Enums;
using RequirementAI.Contract.Settings;

namespace RequirementAI.Business.Providers.LLM;

public class LLMRouteResolver : ILLMRouteResolver
{
    private readonly LLMSettings _settings;

    public LLMRouteResolver(IConfiguration configuration)
    {
        _settings = configuration.GetSection("LLM").Get<LLMSettings>()
                    ?? throw new InvalidOperationException("The LLM configuration is missing.");
    }

    public ResolvedLLMRoute Resolve(LLMRequestPurpose purpose)
    {
        var route = purpose switch
        {
            LLMRequestPurpose.Refinement => _settings.Routing.Refinement,
            LLMRequestPurpose.Analysis => _settings.Routing.Analysis,
            _ => throw new ArgumentOutOfRangeException(nameof(purpose), purpose, null)
        };

        if (string.IsNullOrWhiteSpace(route.Provider))
            throw new InvalidOperationException($"No provider configured for {purpose}.");

        if (!_settings.Providers.TryGetValue(route.Provider, out var provider))
            throw new InvalidOperationException(
                $"The provider '{route.Provider}' configured for {purpose} does not exist in LLM:Providers.");

        if (string.IsNullOrWhiteSpace(route.Model))
            throw new InvalidOperationException($"No model configured for {purpose}.");

        if (!provider.Models.TryGetValue(route.Model, out var model))
            throw new InvalidOperationException(
                $"The model alias '{route.Model}' configured for {purpose} does not exist " +
                $"for provider '{route.Provider}'.");

        return new ResolvedLLMRoute(route.Provider, provider, model);
    }
}
