using System.Text.Encodings.Web;
using System.Text.Json;
using AutoMapper;
using RequirementAI.Business.Interfaces;
using RequirementAI.Business.Interfaces.QualityAnalysis;
using RequirementAI.Business.Interfaces.Refinement;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Contract.Enums;
using RequirementAI.Persistence.Entities;
using JsonSchema = NJsonSchema.JsonSchema;


namespace RequirementAI.Business.Helpers;

public class PromptBuilder(
    IRefinementTaskProvider refinementTaskProvider,
    IAnalysisTaskProvider analysisTaskProvider,
    IItemContextBuilder itemContextBuilder,
    IMapper mapper) : IPromptBuilder
{
    public LLMRequestDto BuildRefinementPrompt<TEntity, TDto>(TEntity entity, string? customInstructions = null)
    {
        var schema = JsonSchema.FromType<TDto>();
        var task = refinementTaskProvider.FromType<TEntity>();
        var input = JsonSerializer.Serialize(mapper.Map<TDto>(entity), new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
        var context = itemContextBuilder.Build(entity);
        var analysisFeedback = BuildAnalysisFeedback(entity);

        return new LLMRequestDto($"""
                                  You are a senior business analyst and requirements refinement assistant.

                                  TASK:
                                  ---
                                  {task}
                                  ---

                                  INPUT:
                                  ---
                                  {input}
                                  ---

                                  CONTEXT:
                                  ---
                                  {context}
                                  ---

                                  ANALYSIS FEEDBACK:
                                  ---
                                  {analysisFeedback}
                                  ---

                                  CUSTOM INSTRUCTIONS:
                                  ---
                                  {customInstructions ?? "No custom instructions provided."}
                                  ---

                                  OBJECTIVE:
                                  Refine the input into clear, structured, and testable requirements according to the task.

                                  INPUT AND OUTPUT LANGUAGE:
                                  - Detect the primary natural language from the INPUT object itself.
                                  - Write every natural-language string in the response in that same language, including titles, descriptions, acceptance criteria, edge cases, assumptions, and explanations stored in JSON fields.
                                  - The language used by this prompt, the JSON schema, CONTEXT, or CUSTOM INSTRUCTIONS must not change the output language.
                                  - Translate structural phrases and user-story wording into the input language; do not copy English example phrases into non-English output.
                                  - Preserve established domain terms from the input when they should not be translated.

                                  INSTRUCTIONS:
                                  - Follow the task exactly.
                                  - Preserve the original business intent.
                                  - Improve clarity, consistency, completeness, and testability.
                                  - Identify missing details, ambiguities, contradictions, risks, and edge cases when relevant.
                                  - Do not invent unsupported domain-specific facts.
                                  - Use reasonable generic assumptions only when necessary and mark them explicitly as assumptions.
                                  - Keep the response concise and non-redundant.
                                  - Return ONLY raw JSON.
                                  - Do not use markdown.
                                  - Do not include explanations outside JSON.
                                  - The response MUST strictly match the provided JSON schema.
                                  - Follow CUSTOM INSTRUCTIONS unless they conflict with the JSON schema, system constraints, or the original business intent.
                                  - Treat ANALYSIS FEEDBACK as advisory guidance, not as authoritative facts.
                                  - Preserve the identified strengths and address supported weaknesses where possible.
                                  - Apply suggestions only when they are consistent with the INPUT and CONTEXT.
                                  - Do not invent information merely to satisfy analysis feedback.

                                  OUTPUT LANGUAGE:
                                  Same as the primary natural language detected from INPUT.

                                  Write every natural-language JSON value exclusively in the input language.
                                  Ignore languages appearing in CONTEXT, examples, feedback, or schema descriptions.

                                  JSON SCHEMA:
                                  {schema.ToJson()}
                                  """, LLMRequestPurpose.Refinement);
    }

    public LLMRequestDto BuildUserStorySplitPrompt(UserStory userStory, string? customInstructions = null)
    {
        var schema = JsonSchema.FromType<UserStorySplitResultDto>();
        var input = JsonSerializer.Serialize(mapper.Map<UserStoryForLLMDto>(userStory), new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
        var context = itemContextBuilder.Build(userStory);

        return new LLMRequestDto($"""
                                  You are a senior business analyst evaluating whether a refined user story should be split.

                                  TASK:
                                  ---
                                  Decide whether the user story contains multiple independently valuable features or concerns.
                                  Split it only when two or three focused, testable, and independently valuable user stories can be produced.
                                  ---

                                  INPUT:
                                  ---
                                  {input}
                                  ---

                                  CONTEXT:
                                  ---
                                  {context}
                                  ---

                                  CUSTOM INSTRUCTIONS:
                                  ---
                                  {customInstructions ?? "No custom instructions provided."}
                                  ---

                                  INPUT AND OUTPUT LANGUAGE:
                                  - Detect the primary natural language from the INPUT user story itself.
                                  - Write every natural-language string in every returned story in that same language, including titles, descriptions, acceptance criteria, and edge cases.
                                  - The language used by this prompt, the JSON schema, CONTEXT, or CUSTOM INSTRUCTIONS must not change the output language.
                                  - Translate the user-story structure into the input language.
                                  - Do not copy English example phrases into non-English output.
                                  - Preserve established domain terms from the input when they should not be translated.

                                  RULES:
                                  - Always return between one and three complete user stories in the UserStories array.
                                  - If no meaningful split is needed, return exactly one refined user story.
                                  - If a split is needed, return exactly two or three focused user stories.
                                  - Each split story must include a title, a description, acceptance criteria, and edge cases.
                                  - Each description must express the persona, desired action or capability, and resulting benefit or business value, in that order and entirely in the input language.
                                  - Preserve the original persona, language, business intent, and scenario context.
                                  - The split stories must collectively cover the original scope without overlap or duplicated acceptance criteria.
                                  - Do not invent unsupported domain-specific facts.
                                  - Keep every story concise, implementation-neutral, and independently testable.
                                  - Follow CUSTOM INSTRUCTIONS unless they conflict with the schema, these rules, or the original business intent.
                                  - Return ONLY raw JSON that strictly matches the provided JSON schema.
                                  - Do not use escaped control characters such as \u0000-\u001F in string values.
                                  - Do not use markdown or include explanations outside JSON.

                                  OUTPUT LANGUAGE:
                                  Same as the primary natural language detected from INPUT.

                                  Write every natural-language JSON value exclusively in the input language.
                                  Ignore languages appearing in CONTEXT, examples, feedback, or schema descriptions.

                                  JSON SCHEMA:
                                  {schema.ToJson()}
                                  """, LLMRequestPurpose.Refinement);
    }

    public LLMRequestDto BuildAnalysisPrompt<TEntity, TRequestDto, TResponseDto>(TEntity entity)
    {
        var schema = JsonSchema.FromType<TResponseDto>();
        var task = analysisTaskProvider.FromType<TEntity>();
        var input = JsonSerializer.Serialize(mapper.Map<TRequestDto>(entity), new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
        var context = itemContextBuilder.Build(entity);

        return new LLMRequestDto($"""
                                  You are a senior business analyst and requirements quality analyst.

                                  TASK:
                                  ---
                                  {task}
                                  ---

                                  INPUT:
                                  ---
                                  {input}
                                  ---

                                  CONTEXT:
                                  ---
                                  {context}
                                  ---

                                  OBJECTIVE:
                                  Analyze the quality of the provided requirement artifact and produce a structured quality evaluation.

                                  INPUT AND OUTPUT LANGUAGE:
                                  - Detect the primary natural language from the INPUT object itself.
                                  - Write every natural-language string in the response in that same language, including strengths, weaknesses, suggestions, risks, and explanations stored in JSON fields.
                                  - The language used by this prompt, the JSON schema, or CONTEXT must not change the output language.
                                  - Preserve established domain terms from the input when they should not be translated.

                                  INSTRUCTIONS:
                                  - Follow the task exactly.
                                  - Evaluate the input as it is; do not refine, rewrite, or generate improved requirement content.
                                  - Score every numeric field from 1 to 10, where 1 = very poor and 10 = excellent.
                                  - Preserve the original business intent.
                                  - Score all requested quality dimensions consistently on the required scale.
                                  - Identify strengths, weaknesses, ambiguities, missing details, contradictions, risks, and edge cases when relevant.
                                  - Do not invent unsupported domain-specific facts.
                                  - Use reasonable generic assumptions only when necessary and mark them explicitly as assumptions.
                                  - Keep the response concise and non-redundant.
                                  - Return ONLY raw JSON.
                                  - Do not use markdown.
                                  - Do not use escaped control characters such as \u0000-\u001F in string values.
                                  - Do not include explanations outside JSON.
                                  - The response MUST strictly match the provided JSON schema.

                                  OUTPUT LANGUAGE:
                                  Same as the primary natural language detected from INPUT.

                                  Write every natural-language JSON value exclusively in the input language.
                                  Ignore languages appearing in CONTEXT, examples, feedback, or schema descriptions.

                                  JSON SCHEMA:
                                  {schema.ToJson()}
                                  """, LLMRequestPurpose.Analysis);
    }

    private static string BuildAnalysisFeedback<TEntity>(TEntity entity)
    {
        QualityScoreBase? latestAnalysis = entity switch
        {
            Persona persona => persona.QualityScores.MaxBy(score => score.CreatedAt),
            Scenario scenario => scenario.QualityScores.MaxBy(score => score.CreatedAt),
            UserStory userStory => userStory.QualityScores.MaxBy(score => score.CreatedAt),
            _ => null
        };

        if (latestAnalysis == null)
            return "No previous analysis available.";

        return $"""
                Strengths:
                {latestAnalysis.Strengths}

                Weaknesses:
                {latestAnalysis.Weaknesses}

                Suggestions:
                {latestAnalysis.Suggestions}
                """;
    }
}
