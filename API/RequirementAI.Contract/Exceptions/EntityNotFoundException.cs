namespace RequirementAI.Contract.Exceptions;

public class EntityNotFoundException<TEntity>(Guid id)
    : PersistenceException($"{typeof(TEntity).Name} with id {id} was not found");