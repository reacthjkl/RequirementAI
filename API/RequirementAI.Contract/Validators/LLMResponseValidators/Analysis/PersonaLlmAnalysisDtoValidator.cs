using FluentValidation;
using RequirementAI.Contract.Dto.LLMDtos;

namespace RequirementAI.Contract.Validators.LLMResponseValidators.Analysis;

public class PersonaLlmAnalysisDtoValidator: AbstractValidator<PersonaLlmAnalysisDto>
{
    public PersonaLlmAnalysisDtoValidator()
    {
        RuleFor(x => x.ClarityScore).GreaterThan(0).LessThanOrEqualTo(10).NotEmpty();
        RuleFor(x => x.RealismScore).GreaterThan(0).LessThanOrEqualTo(10).NotEmpty();
        RuleFor(x => x.GoalClarityScore).GreaterThan(0).LessThanOrEqualTo(10).NotEmpty();
        RuleFor(x => x.PainPointsScore).GreaterThan(0).LessThanOrEqualTo(10).NotEmpty();
        RuleFor(x => x.RelevanceScore).GreaterThan(0).LessThanOrEqualTo(10).NotEmpty();
        RuleFor(x => x.DifferentiationScore).GreaterThan(0).LessThanOrEqualTo(10).NotEmpty();
    }
}