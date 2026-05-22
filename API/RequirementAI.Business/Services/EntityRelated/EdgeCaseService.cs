using AutoMapper;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.EntityRelated;

public class EdgeCaseService(IEdgeCaseRepository edgeCaseRepository, IMapper mapper):IEdgeCaseService
{
    public async Task<EdgeCaseResponseDto> GetById(Guid id, CancellationToken ct)
    {
        var entity = await edgeCaseRepository.GetById(id, ct);
        return mapper.Map<EdgeCaseResponseDto>(entity);
    }

    public async Task<List<EdgeCaseResponseDto>> GetByUserStoryId(Guid userStoryId, CancellationToken ct)
    {
        var entities = await edgeCaseRepository.GetByUserStoryId(userStoryId, ct);
        return mapper.Map<List<EdgeCaseResponseDto>>(entities);
    }

    public async Task<EdgeCaseResponseDto> Create(EdgeCaseForCreationDto edgeCase, CancellationToken ct)
    {
        var entity = mapper.Map<EdgeCase>(edgeCase);

        var created = await edgeCaseRepository.Create(entity, ct);

        return mapper.Map<EdgeCaseResponseDto>(created);
    }

    public async Task Update(EdgeCaseForUpdateDto edgeCase, CancellationToken ct)
    {
        var entity = await edgeCaseRepository.GetById(edgeCase.Id, ct);
        
        mapper.Map(edgeCase, entity);

        await edgeCaseRepository.Update(entity, ct);
    }

    public async Task Delete(Guid id, CancellationToken ct)
    {
        var entity = await edgeCaseRepository.GetById(id, ct);
        await edgeCaseRepository.Delete(entity, ct);
    }
}