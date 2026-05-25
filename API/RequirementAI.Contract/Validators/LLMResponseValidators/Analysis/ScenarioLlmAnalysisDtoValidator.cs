using FluentValidation;
using RequirementAI.Contract.Dto.LLMDtos;

namespace RequirementAI.Contract.Validators.LLMResponseValidators.Analysis;

public class ScenarioLlmAnalysisDtoValidator: AbstractValidator<ScenarioLlmAnalysisDto>
{
    public ScenarioLlmAnalysisDtoValidator()
    {
        RuleFor(x => x.OverallScore).GreaterThan(0).LessThan(10).NotEmpty();
        RuleFor(x => x.ClarityScore).GreaterThan(0).LessThan(10).NotEmpty();
        RuleFor(x => x.ContextScore).GreaterThan(0).LessThan(10).NotEmpty();
        RuleFor(x => x.TriggerScore).GreaterThan(0).LessThan(10).NotEmpty();
        RuleFor(x => x.FlowCompletenessScore).GreaterThan(0).LessThan(10).NotEmpty();
        RuleFor(x => x.EdgeCasesScore).GreaterThan(0).LessThan(10).NotEmpty();
        RuleFor(x => x.PersonaFitScore).GreaterThan(0).LessThan(10).NotEmpty();
    }
}