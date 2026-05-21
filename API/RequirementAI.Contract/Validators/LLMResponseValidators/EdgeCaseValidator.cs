using FluentValidation;
using RequirementAI.Contract.Dto.LLMDtos;

namespace RequirementAI.Contract.Validators.LLMResponseValidators;

public class EdgeCaseValidator: AbstractValidator<EdgeCaseForLLMDto>
{
    public EdgeCaseValidator()
    {
        RuleFor(x => x.Preconditions).MaximumLength(1028).NotEmpty();
        RuleFor(x => x.TriggerAction).MaximumLength(1028).NotEmpty();
        RuleFor(x => x.ExpectedBehavior).MaximumLength(1028).NotEmpty();
    }
}