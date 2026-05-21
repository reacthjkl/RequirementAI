using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Services.EntityRelated;

public class ProjectService: IProjectService
{
    public async Task<List<ProjectResponseDto>> GetByOrganizationId(Guid organizationId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}