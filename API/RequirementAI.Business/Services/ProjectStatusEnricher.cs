using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Enums;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services;

public class ProjectStatusEnricher(
    IProjectRepository projectRepository,
    IJobRepository<ProjectRefinementJob> refinementJobRepository
) : IProjectStatusEnricher
{
    public async Task EnrichAsync(ProjectResponseDto project, CancellationToken ct)
    {
        await EnrichRangeAsync([project], ct);
    }

    public async Task EnrichRangeAsync(List<ProjectResponseDto> projects, CancellationToken ct)
    {
        if (projects.Count == 0) return;

        var projectIds = projects.Select(p => p.Id).ToList();

        var completenessByProjectId =
            await projectRepository.GetCompletenessByProjectIds(projectIds, ct);

        var latestJobStatusByProjectId =
            await refinementJobRepository.GetLatestStatusesByProjectIds(projectIds, ct);

        foreach (var project in projects)
        {
            project.Status = completenessByProjectId.TryGetValue(project.Id, out var isComplete) && isComplete
                ? ProjectStatus.Complete
                : ProjectStatus.Incomplete;

            project.RefinementStatus = latestJobStatusByProjectId.TryGetValue(project.Id, out var jobStatus)
                ? MapRefinementStatus(jobStatus)
                : RefinementStatus.None;
        }
    }

    private static RefinementStatus MapRefinementStatus(JobStatus status)
    {
        return status switch
        {
            JobStatus.Pending => RefinementStatus.Pending,
            JobStatus.Running => RefinementStatus.InProcess,
            JobStatus.Failed => RefinementStatus.Failed,
            JobStatus.Completed => RefinementStatus.Completed,
            _ => RefinementStatus.None
        };
    }
}