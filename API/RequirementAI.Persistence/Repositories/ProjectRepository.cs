using Microsoft.EntityFrameworkCore;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;
using RequirementAI.Persistence.Projections;

namespace RequirementAI.Persistence.Repositories;

public class ProjectRepository(RequirementAIContext context): IProjectRepository
{
    public async Task<Project> GetById(Guid id, CancellationToken ct)
    {
        return await context.Projects.FirstOrDefaultAsync(p => p.Id == id, ct)
            ?? throw new EntityNotFoundException<Project>(id);
    }

    public async Task<Project> GetById(Guid id, Guid organizationId, CancellationToken ct)
    {
        return await context.Projects
                   .FirstOrDefaultAsync(p => p.Id == id && p.OrganizationId == organizationId, ct)
               ?? throw new EntityNotFoundException<Project>(id);
    }

    public async Task<Dictionary<Guid, bool>> GetCompletenessByProjectIds(List<Guid> projectIds, CancellationToken ct)
    {
        var projectsWithPersonas = await context.Personas
            .Where(p => projectIds.Contains(p.ProjectId))
            .Select(p => p.ProjectId)
            .Distinct()
            .ToListAsync(ct);

        var projectsWithPersonaWithoutScenarios = await context.Personas
            .Where(p => projectIds.Contains(p.ProjectId) && !p.Scenarios.Any())
            .Select(p => p.ProjectId)
            .Distinct()
            .ToListAsync(ct);

        var projectsWithScenarioWithoutUserStories = await context.Scenarios
            .Where(s => projectIds.Contains(s.Persona.ProjectId) && !s.UserStories.Any())
            .Select(s => s.Persona.ProjectId)
            .Distinct()
            .ToListAsync(ct);

        var hasPersonas = projectsWithPersonas.ToHashSet();
        var hasPersonaWithoutScenarios = projectsWithPersonaWithoutScenarios.ToHashSet();
        var hasScenarioWithoutUserStories = projectsWithScenarioWithoutUserStories.ToHashSet();

        return projectIds.ToDictionary(
            projectId => projectId,
            projectId =>
                hasPersonas.Contains(projectId)
                && !hasPersonaWithoutScenarios.Contains(projectId)
                && !hasScenarioWithoutUserStories.Contains(projectId));    
    }

    public async Task<Project> GetFullProjectById(Guid id, CancellationToken ct)
    {
        return await GetFullProjectQuery()
                   .FirstOrDefaultAsync(p => p.Id == id, ct)
               ?? throw new EntityNotFoundException<Project>(id);
    }

    public async Task<Project> GetFullProjectById(Guid id, Guid organizationId, CancellationToken ct)
    {
        return await GetFullProjectQuery()
                   .FirstOrDefaultAsync(p => p.Id == id && p.OrganizationId == organizationId, ct)
               ?? throw new EntityNotFoundException<Project>(id);
    }

    public async Task<ProjectUserStoryDetailCountProjection> GetUserStoryDetailCounts(
        Guid id,
        Guid organizationId,
        CancellationToken ct)
    {
        return await context.Projects
                   .Where(project => project.Id == id && project.OrganizationId == organizationId)
                   .Select(project => new ProjectUserStoryDetailCountProjection
                   {
                       ProjectId = project.Id,
                       UserStoryCount = project.Personas
                           .SelectMany(persona => persona.Scenarios)
                           .SelectMany(scenario => scenario.UserStories)
                           .Count(),
                       TotalAcceptanceCriteria = project.Personas
                           .SelectMany(persona => persona.Scenarios)
                           .SelectMany(scenario => scenario.UserStories)
                           .SelectMany(userStory => userStory.AcceptanceCriteria)
                           .Count(),
                       TotalEdgeCases = project.Personas
                           .SelectMany(persona => persona.Scenarios)
                           .SelectMany(scenario => scenario.UserStories)
                           .SelectMany(userStory => userStory.EdgeCases)
                           .Count()
                   })
                   .FirstOrDefaultAsync(ct)
               ?? throw new EntityNotFoundException<Project>(id);
    }

    private IQueryable<Project> GetFullProjectQuery()
    {
        return context.Projects
                   .Include(p => p.Personas)
                   .ThenInclude(p => p.Scenarios)
                   .ThenInclude(s => s.UserStories)
                   .ThenInclude(us => us.AcceptanceCriteria)

                   .Include(p => p.Personas)
                   .ThenInclude(p => p.Scenarios)
                   .ThenInclude(s => s.UserStories)
                   .ThenInclude(us => us.EdgeCases)

                   .Include(p => p.Personas)
                   .ThenInclude(p => p.QualityScores
                       .OrderByDescending(score => score.CreatedAt)
                       .Take(1))

                   .Include(p => p.Personas)
                   .ThenInclude(p => p.Scenarios)
                   .ThenInclude(s => s.QualityScores
                       .OrderByDescending(score => score.CreatedAt)
                       .Take(1))

                   .Include(p => p.Personas)
                   .ThenInclude(p => p.Scenarios)
                   .ThenInclude(s => s.UserStories)
                   .ThenInclude(us => us.QualityScores
                       .OrderByDescending(score => score.CreatedAt)
                       .Take(1))
                   
                   .AsSplitQuery();
    }

    public async Task<DateTimeOffset> GetLatestContentUpdate(Guid id, CancellationToken ct)
    {
        var updates = context.Projects
            .Where(project => project.Id == id)
            .Select(project => project.UpdatedAt)
            .Concat(context.Personas
                .Where(persona => persona.ProjectId == id)
                .Select(persona => persona.UpdatedAt))
            .Concat(context.Scenarios
                .Where(scenario => scenario.Persona.ProjectId == id)
                .Select(scenario => scenario.UpdatedAt))
            .Concat(context.UserStories
                .Where(userStory => userStory.Scenario.Persona.ProjectId == id)
                .Select(userStory => userStory.UpdatedAt))
            .Concat(context.AcceptanceCriteria
                .Where(criterion => criterion.UserStory.Scenario.Persona.ProjectId == id)
                .Select(criterion => criterion.UpdatedAt))
            .Concat(context.EdgeCases
                .Where(edgeCase => edgeCase.UserStory.Scenario.Persona.ProjectId == id)
                .Select(edgeCase => edgeCase.UpdatedAt));

        return await updates.MaxAsync(ct);
    }

    public async Task<IList<Project>> GetByOrganization(Guid organizationId, CancellationToken ct)
    {
        return await context.Projects
            .Where(p => p.OrganizationId == organizationId)
            .ToListAsync(ct);
    }

    public async Task<Project> Create(Project project, CancellationToken ct)
    {
        await context.Projects.AddAsync(project, ct);
        await context.SaveChangesAsync(ct);
        
        return project;
    }

    public async Task<Project> Update(Project project, CancellationToken ct)
    {
        context.Projects.Update(project);
        await context.SaveChangesAsync(ct);
        
        return project;
    }

    public async Task Delete(Project project, CancellationToken ct)
    {
        context.Projects.Remove(project);
        await context.SaveChangesAsync(ct);
    }
}
