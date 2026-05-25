using FluentValidation;
using RequirementAI.Contract.Dto.LLMDtos;

namespace RequirementAI.Contract.Validators.LLMResponseValidators.Refinement;

public class ScenarioForLLMDtoValidator: AbstractValidator<ScenarioForLLMDto>
{
    public ScenarioForLLMDtoValidator()
    {
        RuleFor(x => x.Title).MaximumLength(1028).NotEmpty();
        RuleFor(x => x.Content).MaximumLength(5000).NotEmpty();
    }
}