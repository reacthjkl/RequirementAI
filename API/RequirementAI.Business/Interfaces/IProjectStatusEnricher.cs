using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces;

public interface IProjectStatusEnricher
{
    Task EnrichAsync(ProjectResponseDto project, CancellationToken ct);
    Task EnrichRangeAsync(List<ProjectResponseDto> projects, CancellationToken ct);
}