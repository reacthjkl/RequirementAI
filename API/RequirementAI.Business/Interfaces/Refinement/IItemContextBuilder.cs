
namespace RequirementAI.Business.Interfaces.Refinement;

public interface IItemContextBuilder
{
    string Build<T>(T entity);
}