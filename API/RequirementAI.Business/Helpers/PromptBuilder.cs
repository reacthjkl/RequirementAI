using Newtonsoft.Json.Schema;
using RequirementAI.Contract.Dto;
using NJsonSchema;
using RequirementAI.Business.Interfaces;
using RequirementAI.Business.Interfaces.Refinement;
using RequirementAI.Contract.Dto.LLMDtos;
using JsonSchema = NJsonSchema.JsonSchema;


namespace RequirementAI.Business.Helpers;

public class PromptBuilder(IRefinementTaskProvider refinementTaskProvider): IPromptBuilder
{
    public LLMRequestDto Build<T>(string input, string context)
    {
        var schema = JsonSchema.FromType<T>();
        var task = refinementTaskProvider.FromType<T>();

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

                                  OBJECTIVE:
                                  Analyze the provided input and transform vague, incomplete, inconsistent,
                                  or low-quality information into well-structured requirements.

                                  INSTRUCTIONS:
                                  - Improve and refine the provided input.
                                  - Identify missing details, ambiguities, contradictions, and risks.
                                  - Generate clear and actionable requirements.
                                  - Generate relevant user stories, validation rules, edge cases,
                                    assumptions, and acceptance criteria when applicable.
                                  - Preserve the original business intent.
                                  - Do not invent domain-specific facts that are unsupported by the input.
                                  - If information is missing, make reasonable generic assumptions and
                                    explicitly mark them as assumptions.
                                  - Structure the response logically and consistently.
                                  - Return ONLY raw JSON.
                                  - Do not wrap the response in markdown.
                                  - Do not include explanations or additional text outside JSON.
                                  - The response MUST strictly match the provided JSON schema.

                                  JSON SCHEMA:
                                  {schema.ToJson()}
                                  """);
    }
}