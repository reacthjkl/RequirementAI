using FluentValidation;
using RequirementAI.Contract.Dto.LLMDtos;

namespace RequirementAI.Contract.Validators.LLMResponseValidators;

public class PersonaValidator: AbstractValidator<PersonaForLLMDto>
{
    public PersonaValidator()
    {
        RuleFor(x => x.Name).MaximumLength(255).NotEmpty();
        RuleFor(x => x.Description).MaximumLength(2048).NotEmpty();
        RuleFor(x => x.ContextOfUse).MaximumLength(2048).NotEmpty();
        RuleFor(x => x.Goals).MaximumLength(2048).NotEmpty();
        RuleFor(x => x.Frustrations).MaximumLength(2048).NotEmpty();
    }
}