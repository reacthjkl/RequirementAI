using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RequirementAI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RecalculateOverallScoresAsUnweightedAverage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                UPDATE "PersonaQualityScores"
                SET "OverallScore" = ROUND(
                    (
                        "ClarityScore" +
                        "RealismScore" +
                        "GoalClarityScore" +
                        "PainPointsScore" +
                        "RelevanceScore" +
                        "DifferentiationScore"
                    ) / 6.0,
                    2);
                """);

            migrationBuilder.Sql(
                """
                UPDATE "ScenarioQualityScores"
                SET "OverallScore" = ROUND(
                    (
                        "ClarityScore" +
                        "ContextScore" +
                        "TriggerScore" +
                        "FlowCompletenessScore" +
                        "EdgeCasesScore" +
                        "PersonaFitScore"
                    ) / 6.0,
                    2);
                """);

            migrationBuilder.Sql(
                """
                UPDATE "UserStoryQualityScores"
                SET "OverallScore" = ROUND(
                    (
                        "ClarityScore" +
                        "CompletenessScore" +
                        "TestabilityScore" +
                        "AcceptanceCriteriaScore" +
                        "ScopeScore" +
                        "BusinessValueScore" +
                        "AmbiguityScore"
                    ) / 7.0,
                    2);
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                UPDATE "PersonaQualityScores"
                SET "OverallScore" = ROUND(
                    "ClarityScore" * 0.20 +
                    "RealismScore" * 0.15 +
                    "GoalClarityScore" * 0.25 +
                    "PainPointsScore" * 0.15 +
                    "RelevanceScore" * 0.20 +
                    "DifferentiationScore" * 0.05,
                    2);
                """);

            migrationBuilder.Sql(
                """
                UPDATE "ScenarioQualityScores"
                SET "OverallScore" = ROUND(
                    "ClarityScore" * 0.15 +
                    "ContextScore" * 0.20 +
                    "TriggerScore" * 0.15 +
                    "FlowCompletenessScore" * 0.25 +
                    "EdgeCasesScore" * 0.15 +
                    "PersonaFitScore" * 0.10,
                    2);
                """);

            migrationBuilder.Sql(
                """
                UPDATE "UserStoryQualityScores"
                SET "OverallScore" = ROUND(
                    "ClarityScore" * 0.15 +
                    "CompletenessScore" * 0.20 +
                    "TestabilityScore" * 0.25 +
                    "AcceptanceCriteriaScore" * 0.20 +
                    "ScopeScore" * 0.10 +
                    "BusinessValueScore" * 0.07 +
                    "AmbiguityScore" * 0.03,
                    2);
                """);
        }
    }
}
