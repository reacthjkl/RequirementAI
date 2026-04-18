using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces;

public interface ILLMProvider
{
    Task<List<T>> Generate<T>(string description, CancellationToken ct);
}