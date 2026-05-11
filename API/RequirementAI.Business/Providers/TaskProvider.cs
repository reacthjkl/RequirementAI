using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Providers;

public class TaskProvider: ITaskProvider
{
    private static readonly Dictionary<Type, string> Tasks = new()
    {
        [typeof(UserStoryDto)] =
            "Convert the project description into a user story."
    };
    
    public string FromType<T>()
    {
        if (Tasks.TryGetValue(typeof(T), out var task))
            return task;

        throw new InvalidOperationException(
            $"No task registered for type {typeof(T).Name}");
    }
}