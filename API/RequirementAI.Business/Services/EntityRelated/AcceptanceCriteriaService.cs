using AutoMapper;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.EntityRelated;

public class AcceptanceCriteriaService(IAcceptanceCriteriaRepository acceptanceCriteriaRepository, IMapper mapper): IAcceptanceCriteriaService
{
    public async Task<AcceptanceCriteriaResponseDto> GetById(Guid id, CancellationToken ct)
    {
        var entity = await acceptanceCriteriaRepository.GetById(id, ct);
        return mapper.Map<AcceptanceCriteriaResponseDto>(entity);
    }

    public async Task<List<AcceptanceCriteriaResponseDto>> GetByUserStoryId(Guid userStoryId, CancellationToken ct)
    {
        var entities = await acceptanceCriteriaRepository.GetByUserStoryId(userStoryId, ct);
        return mapper.Map<List<AcceptanceCriteriaResponseDto>>(entities);
    }

    public async Task<AcceptanceCriteriaResponseDto> Create(
        AcceptanceCriteriaForCreationDto dto,
        CancellationToken ct)
    {
        var entity = mapper.Map<AcceptanceCriteria>(dto);

        var created = await acceptanceCriteriaRepository.Create(entity, ct);

        return mapper.Map<AcceptanceCriteriaResponseDto>(created);
    }

    public async Task Update(
        AcceptanceCriteriaForUpdateDto dto,
        CancellationToken ct)
    {
        var entity = mapper.Map<AcceptanceCriteria>(dto);

        await acceptanceCriteriaRepository.Update(entity, ct);
    }

    public async Task Delete(Guid id, CancellationToken ct)
    {
        var entity = await acceptanceCriteriaRepository.GetById(id, ct);
        await acceptanceCriteriaRepository.Delete(entity, ct);
    }
}