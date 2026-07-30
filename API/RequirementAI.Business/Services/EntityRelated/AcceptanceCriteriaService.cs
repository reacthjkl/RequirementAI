using AutoMapper;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.EntityRelated;

public class AcceptanceCriteriaService(
    IAcceptanceCriteriaRepository acceptanceCriteriaRepository,
    IUserStoryRepository userStoryRepository,
    IMapper mapper): IAcceptanceCriteriaService
{
    public async Task<AcceptanceCriteriaResponseDto> GetById(Guid id, Guid organizationId, CancellationToken ct)
    {
        var entity = await acceptanceCriteriaRepository.GetById(id, organizationId, ct);
        return mapper.Map<AcceptanceCriteriaResponseDto>(entity);
    }

    public async Task<List<AcceptanceCriteriaResponseDto>> GetByUserStoryId(Guid userStoryId, Guid organizationId, CancellationToken ct)
    {
        var entities = await acceptanceCriteriaRepository.GetByUserStoryId(userStoryId, organizationId, ct);
        return mapper.Map<List<AcceptanceCriteriaResponseDto>>(entities);
    }

    public async Task<AcceptanceCriteriaResponseDto> Create(
        AcceptanceCriteriaForCreationDto dto,
        Guid organizationId,
        CancellationToken ct)
    {
        await userStoryRepository.GetById(dto.UserStoryId, organizationId, ct);

        var entity = mapper.Map<AcceptanceCriteria>(dto);

        var created = await acceptanceCriteriaRepository.Create(entity, ct);

        return mapper.Map<AcceptanceCriteriaResponseDto>(created);
    }

    public async Task Update(
        AcceptanceCriteriaForUpdateDto dto,
        Guid organizationId,
        CancellationToken ct)
    {
        var entity = await acceptanceCriteriaRepository.GetById(dto.Id, organizationId, ct);
        
        mapper.Map(dto, entity);
        
        await acceptanceCriteriaRepository.Update(entity, ct);
    }

    public async Task Delete(Guid id, Guid organizationId, CancellationToken ct)
    {
        var entity = await acceptanceCriteriaRepository.GetById(id, organizationId, ct);
        await acceptanceCriteriaRepository.Delete(entity, ct);
    }
}
