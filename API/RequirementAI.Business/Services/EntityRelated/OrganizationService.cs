using AutoMapper;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.EntityRelated;

public class OrganizationService(IOrganizationRepository organizationRepository, IMapper mapper): IOrganizationService
{
    public async Task<OrganizationResponseDto> GetById(Guid id, CancellationToken ct)
    {
        var entity = await organizationRepository.GetById(id, ct);
        return mapper.Map<OrganizationResponseDto>(entity);
    }

    public async Task Update(OrganizationForUpdateDto organization, CancellationToken ct)
    {
        var entity = mapper.Map<Organization>(organization);

        await organizationRepository.Update(entity, ct);
    }
}