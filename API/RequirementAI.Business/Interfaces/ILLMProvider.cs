using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces;

public interface ILLMProvider
{
    Task<T> Generate<T>(string description, CancellationToken ct);
}