using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RequirementAI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class StoreQualityScoresWithTwoDecimalPlaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TestabilityScore",
                table: "UserStoryQualityScores",
                type: "numeric(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "ScopeScore",
                table: "UserStoryQualityScores",
                type: "numeric(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "OverallScore",
                table: "UserStoryQualityScores",
                type: "numeric(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "CompletenessScore",
                table: "UserStoryQualityScores",
                type: "numeric(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "ClarityScore",
                table: "UserStoryQualityScores",
                type: "numeric(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "BusinessValueScore",
                table: "UserStoryQualityScores",
                type: "numeric(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "AmbiguityScore",
                table: "UserStoryQualityScores",
                type: "numeric(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "AcceptanceCriteriaScore",
                table: "UserStoryQualityScores",
                type: "numeric(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "TriggerScore",
                table: "ScenarioQualityScores",
                type: "numeric(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "PersonaFitScore",
                table: "ScenarioQualityScores",
                type: "numeric(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "OverallScore",
                table: "ScenarioQualityScores",
                type: "numeric(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "FlowCompletenessScore",
                table: "ScenarioQualityScores",
                type: "numeric(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "EdgeCasesScore",
                table: "ScenarioQualityScores",
                type: "numeric(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "ContextScore",
                table: "ScenarioQualityScores",
                type: "numeric(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "ClarityScore",
                table: "ScenarioQualityScores",
                type: "numeric(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "RelevanceScore",
                table: "PersonaQualityScores",
                type: "numeric(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "RealismScore",
                table: "PersonaQualityScores",
                type: "numeric(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "PainPointsScore",
                table: "PersonaQualityScores",
                type: "numeric(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "OverallScore",
                table: "PersonaQualityScores",
                type: "numeric(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "GoalClarityScore",
                table: "PersonaQualityScores",
                type: "numeric(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "DifferentiationScore",
                table: "PersonaQualityScores",
                type: "numeric(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "ClarityScore",
                table: "PersonaQualityScores",
                type: "numeric(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TestabilityScore",
                table: "UserStoryQualityScores",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "ScopeScore",
                table: "UserStoryQualityScores",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "OverallScore",
                table: "UserStoryQualityScores",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "CompletenessScore",
                table: "UserStoryQualityScores",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "ClarityScore",
                table: "UserStoryQualityScores",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "BusinessValueScore",
                table: "UserStoryQualityScores",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "AmbiguityScore",
                table: "UserStoryQualityScores",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "AcceptanceCriteriaScore",
                table: "UserStoryQualityScores",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "TriggerScore",
                table: "ScenarioQualityScores",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "PersonaFitScore",
                table: "ScenarioQualityScores",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "OverallScore",
                table: "ScenarioQualityScores",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "FlowCompletenessScore",
                table: "ScenarioQualityScores",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "EdgeCasesScore",
                table: "ScenarioQualityScores",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "ContextScore",
                table: "ScenarioQualityScores",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "ClarityScore",
                table: "ScenarioQualityScores",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "RelevanceScore",
                table: "PersonaQualityScores",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "RealismScore",
                table: "PersonaQualityScores",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "PainPointsScore",
                table: "PersonaQualityScores",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "OverallScore",
                table: "PersonaQualityScores",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "GoalClarityScore",
                table: "PersonaQualityScores",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "DifferentiationScore",
                table: "PersonaQualityScores",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "ClarityScore",
                table: "PersonaQualityScores",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,2)",
                oldPrecision: 4,
                oldScale: 2);
        }
    }
}
