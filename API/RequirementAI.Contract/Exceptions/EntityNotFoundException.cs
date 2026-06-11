namespace RequirementAI.Contract.Exceptions;

public abstract class EntityNotFoundException(string message)
    : PersistenceException(message);

public class EntityNotFoundException<TEntity>(Guid id)
    : EntityNotFoundException($"{typeof(TEntity).Name} with id {id} was not found");
