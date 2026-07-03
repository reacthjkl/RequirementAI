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
                         
                         Rules:
                         - The persona name must be a realistic human first name, optionally with a last name.
                         - Do not use roles, job titles, generic labels, or descriptions as persona names.
                         - Invalid names: "Student", "User", "Customer", "Developer", "Persona 1".
                         - Valid names: "Lena", "Markus Weber", "Anna Schmidt".
                         
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
                         
                         Length limits:
                         - Scenario title: max. 80 characters
                         - Scenario description: max. 6 sentences
                         - Each sentence: max. 25 words
                         - Do not add background story unless needed for requirements.
                         
                         Rules:
                         - The scenario MUST use only the provided persona as the main actor.
                         - If the input contains another person name, replace it with the bound persona name.
                         - Do not merge multiple personas into one scenario.
                         
                         Keep the output concise, structured, and testable.
                         """,

        [typeof(UserStory)] = """
                         Refine the user story into a clear, testable format.
                         
                         The user story description must strictly follow the localized equivalent of this schema:

                         As a [persona], I want [action/capability], so that [benefit/business value].

                         Translate the structural phrases "As a", "I want", and "so that" into the input language.
                         For German input, use: Als [Persona] möchte ich [Aktion/Fähigkeit], damit [Nutzen/Mehrwert].
                         
                         Also include:
                         - Acceptance criteria
                         - Edge cases
                         
                         Rules:
                         - The user story MUST use only the provided persona as the main actor.
                         - If the input contains another person name, replace it with the bound persona name.
                         - Do not merge multiple personas into one scenario.
                         
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
