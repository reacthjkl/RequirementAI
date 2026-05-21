namespace RequirementAI.Business.Interfaces.Refinement;

public interface IRefinementMerger<TEntity, TDto>
{
    public void Apply(TEntity entity, TDto dto);
}