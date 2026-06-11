using FluentValidation;
using RequirementAI.Contract.Dto.LLMDtos;

namespace RequirementAI.Contract.Validators.LLMResponseValidators.Analysis;

public class UserStoryLlmAnalysisDtoValidator: AbstractValidator<UserStoryLlmAnalysisDto>
{
    public UserStoryLlmAnalysisDtoValidator()
    {
        RuleFor(x => x.ClarityScore).GreaterThan(0).LessThanOrEqualTo(10).NotEmpty();
        RuleFor(x => x.CompletenessScore).GreaterThan(0).LessThanOrEqualTo(10).NotEmpty();
        RuleFor(x => x.TestabilityScore).GreaterThan(0).LessThanOrEqualTo(10).NotEmpty();
        RuleFor(x => x.AcceptanceCriteriaScore).GreaterThan(0).LessThanOrEqualTo(10).NotEmpty();
        RuleFor(x => x.ScopeScore).GreaterThan(0).LessThanOrEqualTo(10).NotEmpty();
        RuleFor(x => x.BusinessValueScore).GreaterThan(0).LessThanOrEqualTo(10).NotEmpty();
        RuleFor(x => x.AmbiguityScore).GreaterThan(0).LessThanOrEqualTo(10).NotEmpty();
    }
}