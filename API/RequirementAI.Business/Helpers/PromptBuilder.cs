using System.Text.Json;
using AutoMapper;
using RequirementAI.Business.Interfaces;
using RequirementAI.Business.Interfaces.Refinement;
using RequirementAI.Contract.Dto.LLMDtos;
using JsonSchema = NJsonSchema.JsonSchema;


namespace RequirementAI.Business.Helpers;

public class PromptBuilder(IRefinementTaskProvider refinementTaskProvider, IRefinementContextBuilder refinementContextBuilder, IMapper mapper): IPromptBuilder
{
    public LLMRequestDto Build<TEntity, TDto>(TEntity entity, string? customInstructions = null)
    {
        var schema = JsonSchema.FromType<TDto>();
        var task = refinementTaskProvider.FromType<TEntity>();
        var input = JsonSerializer.Serialize(mapper.Map<TDto>(entity));
        var context = refinementContextBuilder.Build(entity);

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
                                   - Do not include explanations outside JSON.
                                   - The response MUST strictly match the provided JSON schema.
                                   - Follow CUSTOM INSTRUCTIONS unless they conflict with the JSON schema, system constraints, or the original business intent.
                                   
                                   JSON SCHEMA:
                                   {schema.ToJson()}
                                   """);
    }
}