using FluentValidation;
using RequirementAI.Contract.Dto.LLMDtos;

namespace RequirementAI.Contract.Validators.LLMResponseValidators;

public class AcceptanceCriteriaValidator: AbstractValidator<AcceptanceCriteriaForLLMDto>
{
    public AcceptanceCriteriaValidator()
    {
        RuleFor(x => x.Wording).MaximumLength(1028).NotEmpty();
    }
}