using RequirementAI.Business.Interfaces;
using RequirementAI.Business.Interfaces.Refinement;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Providers;

public class RefinementTaskProvider : IRefinementTaskProvider
{
    private static readonly Dictionary<Type, string> Tasks = new()
    {
        [typeof(Persona)] = """
                         Refine the provided persona for requirements engineering.
                         
                         Ensure the persona is:
                         - role-based
                         - realistic
                         - goal-oriented
                         - behavior-focused
                         - distinct from other personas
                         - implementation-neutral
                         
                         Improve:
                         - clarity
                         - consistency
                         - completeness
                         - motivations
                         - goals
                         - pain points
                         - expectations
                         - context of system usage
                         - relevant constraints or limitations
                         
                         Remove irrelevant biographical details unless they affect requirements.
                         
                         Keep the output concise, structured, and actionable for deriving scenarios and user stories.
                         """,

        [typeof(Scenario)] = """
                         Refine the scenario description for requirements engineering.
                         
                         Ensure the scenario is:
                         - logically ordered
                         - user-centered
                         - goal-oriented
                         - realistic
                         - implementation-neutral
                         - specific enough to derive user stories and acceptance criteria
                         
                         Identify and improve:
                         - missing steps
                         - unclear assumptions
                         - dependencies
                         - preconditions
                         - alternative flows
                         - edge cases
                         - expected outcome
                         
                         Keep the output concise, structured, and testable.
                         """,

        [typeof(UserStory)] = """
                         Refine the user story into a clear, testable format.
                         
                         The user story description must strictly follow this schema:
                         
                         As a [persona], I want [action/capability], so that [benefit/business value].
                         
                         Also include:
                         - Acceptance criteria
                         - Edge cases
                         - Testable conditions
                         
                         Keep the output concise, structured, and implementation-neutral.
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