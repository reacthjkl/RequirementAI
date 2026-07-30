using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto.LLMDtos;

namespace RequirementAI.Business.Providers.LLM;

public class RoutingLLMProvider : ILLMProvider
{
    private readonly ILLMRouteResolver _routeResolver;
    private readonly IReadOnlyDictionary<string, ILLMProviderAdapter> _adapters;

    public RoutingLLMProvider(
        ILLMRouteResolver routeResolver,
        IEnumerable<ILLMProviderAdapter> adapters)
    {
        _routeResolver = routeResolver;
        _adapters = adapters.ToDictionary(
            adapter => adapter.ProviderType,
            StringComparer.OrdinalIgnoreCase);
    }

    public Task<string> GetResponse(LLMRequestDto request, CancellationToken ct)
    {
        var route = _routeResolver.Resolve(request.Purpose);

        if (!_adapters.TryGetValue(route.Provider.Type, out var adapter))
            throw new InvalidOperationException(
                $"No LLM adapter is registered for provider type '{route.Provider.Type}'.");

        return adapter.GetResponse(
            route.ProviderId,
            route.Provider,
            route.Model,
            request,
            ct);
    }
}
