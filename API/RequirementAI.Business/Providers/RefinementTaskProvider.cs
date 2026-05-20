using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto.LLMDtos;

namespace RequirementAI.Business.Providers;

public class RefinementTaskProvider : IRefinementTaskProvider
{
    private static readonly Dictionary<Type, string> Tasks = new()
    {
        [typeof(PersonaDto)] = """
                         Refine the provided persona. Improve clarity, consistency, and completeness.
                         Ensure motivations, goals, pain points, and context are well-defined.
                         """,

        [typeof(ScenarioDto)] = """
                         Refine the scenario description. Ensure logical flow, missing steps,
                         edge cases, and dependencies are identified.
                         """,

        [typeof(UserStoryDto)] = """
                         Refine the user story into clear format with acceptance criteria,
                         edge cases, and testable conditions.
                         """
    };

    public string FromType<T>()
    {
        if (Tasks.TryGetValue(typeof(T), out var task))
            return task;

        throw new InvalidOperationException(
            $"No refinement task registered for type {typeof(T).Name}");
    }
}