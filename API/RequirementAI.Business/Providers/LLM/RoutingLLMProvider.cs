using Microsoft.Extensions.Configuration;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Contract.Enums;
using RequirementAI.Contract.Settings;

namespace RequirementAI.Business.Providers.LLM;

public class RoutingLLMProvider : ILLMProvider
{
    private readonly LLMSettings _settings;
    private readonly IReadOnlyDictionary<string, ILLMProviderAdapter> _adapters;

    public RoutingLLMProvider(
        IConfiguration configuration,
        IEnumerable<ILLMProviderAdapter> adapters)
    {
        _settings = configuration.GetSection("LLM").Get<LLMSettings>()
                    ?? throw new InvalidOperationException("The LLM configuration is missing.");
        _adapters = adapters.ToDictionary(
            adapter => adapter.ProviderType,
            StringComparer.OrdinalIgnoreCase);
    }

    public Task<string> GetResponse(LLMRequestDto request, CancellationToken ct)
    {
        var purpose = request.Purpose.ToString();
        var route = request.Purpose switch
        {
            LLMRequestPurpose.Refinement => _settings.Routing.Refinement,
            LLMRequestPurpose.Analysis => _settings.Routing.Analysis,
            _ => throw new ArgumentOutOfRangeException(nameof(request.Purpose), request.Purpose, null)
        };

        if (string.IsNullOrWhiteSpace(route.Provider))
            throw new InvalidOperationException($"No provider configured for {purpose}.");

        if (!_settings.Providers.TryGetValue(route.Provider, out var provider))
            throw new InvalidOperationException(
                $"The provider '{route.Provider}' configured for {purpose} does not exist in LLM:Providers.");

        if (!_adapters.TryGetValue(provider.Type, out var adapter))
            throw new InvalidOperationException(
                $"No LLM adapter is registered for provider type '{provider.Type}'.");

        if (string.IsNullOrWhiteSpace(route.Model))
            throw new InvalidOperationException($"No model configured for {purpose}.");

        if (!provider.Models.TryGetValue(route.Model, out var model))
            throw new InvalidOperationException(
                $"The model alias '{route.Model}' configured for {purpose} does not exist " +
                $"for provider '{route.Provider}'.");

        return adapter.GetResponse(route.Provider, provider, model, request, ct);
    }
}
