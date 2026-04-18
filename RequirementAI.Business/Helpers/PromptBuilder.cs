using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Helpers;

public static class PromptBuilder
{
    public static LLMRequestDto UserStory(string description)
    {
        return new LLMRequestDto($$"""
                                   You are a product analyst.

                                   Convert the project description into user stories.

                                   Return JSON in this format:

                                   [
                                       {
                                         "title": "",
                                         "description": "",
                                         "acceptanceCriteria": []
                                        }
                                   ]
                                   
                                   Do not add anything before and after the brackets.

                                   Description:
                                   {{description}}
                                   """);
    }
}