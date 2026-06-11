using System.Text.Json;
using AutoMapper;
using RequirementAI.Business.Interfaces;
using RequirementAI.Business.Interfaces.QualityAnalysis;
using RequirementAI.Business.Interfaces.Refinement;
using RequirementAI.Contract.Dto.LLMDtos;
using JsonSchema = NJsonSchema.JsonSchema;


namespace RequirementAI.Business.Helpers;

public class PromptBuilder(
    IRefinementTaskProvider refinementTaskProvider, 
    IAnalysisTaskProvider analysisTaskProvider, 
    IItemContextBuilder itemContextBuilder, 
    IMapper mapper): IPromptBuilder
{
    public LLMRequestDto BuildRefinementPrompt<TEntity, TDto>(TEntity entity, string? customInstructions = null)
    {
        var schema = JsonSchema.FromType<TDto>();
        var task = refinementTaskProvider.FromType<TEntity>();
        var input = JsonSerializer.Serialize(mapper.Map<TDto>(entity));
        var context = itemContextBuilder.Build(entity);

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
                                   
                                   CUSTOM INSTRUCTIONS:
                                   ---
                                   {customInstructions ?? "No custom instructions provided."}
                                   ---
                                   
                                   OBJECTIVE:
                                   Refine the input into clear, structured, and testable requirements according to the task.
                                   
                                   INSTRUCTIONS:
                                   - Follow the task exactly.
                                   - Preserve the original business intent.
                                   - Write all generated or refined user stories in the same language as the input.
                                   - Improve clarity, consistency, completeness, and testability.
                                   - Identify missing details, ambiguities, contradictions, risks, and edge cases when relevant.
                                   - Do not invent unsupported domain-specific facts.
                                   - Use reasonable generic assumptions only when necessary and mark them explicitly as assumptions.
                                   - Keep the response concise and non-redundant.
                                   - Return ONLY raw JSON.
                                   - Do not use markdown.
                                   - Do not use escaped control characters such as \u0000-\u001F in string values. Use normal UTF-8 German characters like ä, ö, ü, ß.
                                   - Do not include explanations outside JSON.
                                   - The response MUST strictly match the provided JSON schema.
                                   - Follow CUSTOM INSTRUCTIONS unless they conflict with the JSON schema, system constraints, or the original business intent.
                                   
                                   JSON SCHEMA:
                                   {schema.ToJson()}
                                   """);
    }

    public LLMRequestDto BuildAnalysisPrompt<TEntity, TRequestDto, TResponseDto>(TEntity entity)
    {
        var schema = JsonSchema.FromType<TResponseDto>();
        var task = analysisTaskProvider.FromType<TEntity>();
        var input = JsonSerializer.Serialize(mapper.Map<TRequestDto>(entity));
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
                           - Do not use escaped control characters such as \u0000-\u001F in string values. Use normal UTF-8 German characters like ä, ö, ü, ß.
                           - Do not include explanations outside JSON.
                           - The response MUST strictly match the provided JSON schema.
                           
                           JSON SCHEMA:
                           {schema.ToJson()}
                           """);
    }
}