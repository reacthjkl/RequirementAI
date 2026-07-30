using AutoMapper;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.EntityRelated;

public class EdgeCaseService(
    IEdgeCaseRepository edgeCaseRepository,
    IUserStoryRepository userStoryRepository,
    IMapper mapper):IEdgeCaseService
{
    public async Task<EdgeCaseResponseDto> GetById(Guid id, Guid organizationId, CancellationToken ct)
    {
        var entity = await edgeCaseRepository.GetById(id, organizationId, ct);
        return mapper.Map<EdgeCaseResponseDto>(entity);
    }

    public async Task<List<EdgeCaseResponseDto>> GetByUserStoryId(Guid userStoryId, Guid organizationId, CancellationToken ct)
    {
        var entities = await edgeCaseRepository.GetByUserStoryId(userStoryId, organizationId, ct);
        return mapper.Map<List<EdgeCaseResponseDto>>(entities);
    }

    public async Task<EdgeCaseResponseDto> Create(EdgeCaseForCreationDto edgeCase, Guid organizationId, CancellationToken ct)
    {
        await userStoryRepository.GetById(edgeCase.UserStoryId, organizationId, ct);

        var entity = mapper.Map<EdgeCase>(edgeCase);

        var created = await edgeCaseRepository.Create(entity, ct);

        return mapper.Map<EdgeCaseResponseDto>(created);
    }

    public async Task Update(EdgeCaseForUpdateDto edgeCase, Guid organizationId, CancellationToken ct)
    {
        var entity = await edgeCaseRepository.GetById(edgeCase.Id, organizationId, ct);
        
        mapper.Map(edgeCase, entity);

        await edgeCaseRepository.Update(entity, ct);
    }

    public async Task Delete(Guid id, Guid organizationId, CancellationToken ct)
    {
        var entity = await edgeCaseRepository.GetById(id, organizationId, ct);
        await edgeCaseRepository.Delete(entity, ct);
    }
}
