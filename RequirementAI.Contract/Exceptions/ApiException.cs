namespace RequirementAI.Contract.Exceptions;

public abstract class ApiException(string message) : RequirementAIException(message)
{
}