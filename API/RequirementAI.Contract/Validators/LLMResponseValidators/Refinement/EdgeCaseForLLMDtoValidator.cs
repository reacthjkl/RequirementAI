using FluentValidation;
using RequirementAI.Contract.Dto.LLMDtos;

namespace RequirementAI.Contract.Validators.LLMResponseValidators.Refinement;

public class EdgeCaseForLLMDtoValidator: AbstractValidator<EdgeCaseForLLMDto>
{
    public EdgeCaseForLLMDtoValidator()
    {
        RuleFor(x => x.Preconditions).MaximumLength(1028).NotEmpty();
        RuleFor(x => x.TriggerAction).MaximumLength(1028).NotEmpty();
        RuleFor(x => x.ExpectedBehavior).MaximumLength(1028).NotEmpty();
    }
}