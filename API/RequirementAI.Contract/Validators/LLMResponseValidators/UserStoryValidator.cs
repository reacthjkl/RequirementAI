using FluentValidation;
using RequirementAI.Contract.Dto.LLMDtos;

namespace RequirementAI.Contract.Validators.LLMResponseValidators;

public class UserStoryValidator: AbstractValidator<UserStoryForLLMDto>
{
    public UserStoryValidator()
    {
        RuleFor(x => x.Title).MaximumLength(1028).NotEmpty();
        RuleFor(x => x.Description).NotEmpty().MaximumLength(4000);
        
        RuleFor(x => x.AcceptanceCriteria)
            .NotNull()
            .NotEmpty()
            .Must(x => x.Count <= 15);

        RuleForEach(x => x.AcceptanceCriteria)
            .SetValidator(new AcceptanceCriteriaValidator());

        RuleFor(x => x.EdgeCases)
            .NotNull()
            .Must(x => x.Count <= 20);

        RuleForEach(x => x.EdgeCases)
            .SetValidator(new EdgeCaseValidator());
    }
}