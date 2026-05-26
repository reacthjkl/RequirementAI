using FluentValidation;
using RequirementAI.Contract.Dto.LLMDtos;

namespace RequirementAI.Contract.Validators.LLMResponseValidators.Refinement;

public class AcceptanceCriteriaForLLMDtoValidator: AbstractValidator<AcceptanceCriteriaForLLMDto>
{
    public AcceptanceCriteriaForLLMDtoValidator()
    {
        RuleFor(x => x.Wording).MaximumLength(1028).NotEmpty();
    }
}