using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces.EntityRelated;

public interface IOrganizationService
{
    public Task<OrganizationResponseDto> GetById(Guid id, CancellationToken ct);
    public Task Update(OrganizationForUpdateDto organization, CancellationToken ct);
}