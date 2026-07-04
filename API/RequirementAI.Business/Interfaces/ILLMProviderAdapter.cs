using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Contract.Settings;

namespace RequirementAI.Business.Interfaces;

public interface ILLMProviderAdapter
{
    string ProviderType { get; }

    Task<string> GetResponse(
        string providerId,
        LLMProviderSettings provider,
        string model,
        LLMRequestDto request,
        CancellationToken ct);
}
