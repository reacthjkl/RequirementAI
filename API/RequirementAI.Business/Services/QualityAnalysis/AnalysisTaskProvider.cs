using RequirementAI.Business.Interfaces.QualityAnalysis;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Services.QualityAnalysis;

public class AnalysisTaskProvider: IAnalysisTaskProvider
{
    private static readonly Dictionary<Type, string> Tasks = new()
    {
        [typeof(Persona)] = """
            Analyze the provided persona for requirements engineering quality.

            Score the following dimensions from 1 to 10:
            - clarity
            - realism
            - goal clarity
            - pain points quality
            - relevance to the project
            - differentiation from other personas

            Also provide:
            - strengths
            - weaknesses
            - suggestions

            Do not refine or rewrite the persona.
            Return only the quality analysis according to the provided JSON schema.
            """,

        [typeof(Scenario)] = """
            Analyze the provided scenario for requirements engineering quality.

            Score the following dimensions from 1 to 10:
            - clarity
            - context quality
            - trigger quality
            - flow completeness
            - edge case coverage
            - persona fit

            Also provide:
            - strengths
            - weaknesses
            - suggestions

            Do not refine or rewrite the scenario.
            Return only the quality analysis according to the provided JSON schema.
            """,

        [typeof(UserStory)] = """
                              Analyze the provided user story for requirements engineering quality.

                              Score the following dimensions from 1 to 10:
                              - clarity
                              - completeness
                              - testability
                              - acceptance criteria quality
                              - scope size
                              - business value
                              - ambiguity

                              Also provide:
                              - strengths
                              - weaknesses
                              - suggestions

                              Check whether the story clearly expresses, in the input language:
                              - the persona or role
                              - the desired action or capability
                              - the resulting benefit or business value

                              Do not refine or rewrite the user story.
                              Return only the quality analysis according to the provided JSON schema.
                              """
    };

    public string FromType<T>()
    {
        if (Tasks.TryGetValue(typeof(T), out var task))
            return task;

        throw new InvalidOperationException(
            $"No analysis task registered for type {typeof(T).Name}");
    }
}
