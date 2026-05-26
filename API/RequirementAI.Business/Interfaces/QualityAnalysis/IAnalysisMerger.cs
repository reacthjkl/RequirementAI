namespace RequirementAI.Business.Interfaces.QualityAnalysis;

public interface IAnalysisMerger<TEntity, TDto>
{
    public void Apply(TEntity entity, TDto dto);
}