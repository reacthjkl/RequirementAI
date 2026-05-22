
namespace RequirementAI.Business.Interfaces;

public interface IRefinementContextBuilder
{
    string Build<T>(T entity);
}