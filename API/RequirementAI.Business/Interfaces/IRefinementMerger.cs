namespace RequirementAI.Business.Interfaces;

public interface IRefinementMerger<TEntity, TDto>
{
    public void Apply(TEntity entity, TDto dto);
}