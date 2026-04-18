namespace RequirementAI.Contract.Exceptions;

public class RequirementAIException(string message) : Exception(message)
{
    public override string Message => GetType().Name + ": " + base.Message;
}