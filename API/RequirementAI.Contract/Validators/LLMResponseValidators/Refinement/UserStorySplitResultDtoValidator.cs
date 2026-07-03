using FluentValidation;
using RequirementAI.Contract.Dto.LLMDtos;

namespace RequirementAI.Contract.Validators.LLMResponseValidators.Refinement;

public class UserStorySplitResultDtoValidator : AbstractValidator<UserStorySplitResultDto>
{
    public UserStorySplitResultDtoValidator(IValidator<UserStoryForLLMDto> userStoryValidator)
    {
        RuleFor(x => x.UserStories)
            .NotNull()
            .Must(userStories => userStories is { Count: >= 1 and <= 3 })
            .WithMessage("A split result must contain between one and three user stories.");

        RuleForEach(x => x.UserStories).SetValidator(userStoryValidator);
    }
}
