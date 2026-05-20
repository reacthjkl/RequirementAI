using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.LLMDtos;

namespace RequirementAI.Business.Interfaces;

public interface ILLMProvider
{
    public Task<string> GetResponse(LLMRequestDto request, CancellationToken ct);
}