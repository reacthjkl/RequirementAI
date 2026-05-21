using Microsoft.EntityFrameworkCore;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Persistence.Repositories;

public class ProjectRepository(RequirementAIContext context): IProjectRepository
{
    public async Task<Project> GetById(Guid id, CancellationToken ct)
    {
        return await context.Projects.FirstOrDefaultAsync(p => p.Id == id, ct)
            ?? throw new EntityNotFoundException<Project>(id);
    }

    public async Task<Project> GetFullProjectById(Guid id, CancellationToken ct)
    {
        return await context.Projects
                   .Include(p => p.Personas)
                   .ThenInclude(p => p.Scenarios)
                   .ThenInclude(s => s.UserStories)
                   .ThenInclude(us => us.AcceptanceCriteria)

                   .Include(p => p.Personas)
                   .ThenInclude(p => p.Scenarios)
                   .ThenInclude(s => s.UserStories)
                   .ThenInclude(us => us.EdgeCases)
                   
                   .AsSplitQuery()
                   .FirstOrDefaultAsync(p => p.Id == id, ct)
               ?? throw new EntityNotFoundException<Project>(id);
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