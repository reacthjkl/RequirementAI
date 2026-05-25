using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RequirementAI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class QualityScores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonaQualityScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PersonaId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClarityScore = table.Column<int>(type: "integer", nullable: false),
                    RealismScore = table.Column<int>(type: "integer", nullable: false),
                    GoalClarityScore = table.Column<int>(type: "integer", nullable: false),
                    PainPointsScore = table.Column<int>(type: "integer", nullable: false),
                    RelevanceScore = table.Column<int>(type: "integer", nullable: false),
                    DifferentiationScore = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    OverallScore = table.Column<int>(type: "integer", nullable: false),
                    Strengths = table.Column<string>(type: "text", nullable: false),
                    Weaknesses = table.Column<string>(type: "text", nullable: false),
                    Suggestions = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonaQualityScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonaQualityScores_Personas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScenarioQualityScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ScenarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClarityScore = table.Column<int>(type: "integer", nullable: false),
                    ContextScore = table.Column<int>(type: "integer", nullable: false),
                    TriggerScore = table.Column<int>(type: "integer", nullable: false),
                    FlowCompletenessScore = table.Column<int>(type: "integer", nullable: false),
                    EdgeCasesScore = table.Column<int>(type: "integer", nullable: false),
                    PersonaFitScore = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    OverallScore = table.Column<int>(type: "integer", nullable: false),
                    Strengths = table.Column<string>(type: "text", nullable: false),
                    Weaknesses = table.Column<string>(type: "text", nullable: false),
                    Suggestions = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScenarioQualityScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScenarioQualityScores_Scenarios_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "Scenarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStoryQualityScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserStoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClarityScore = table.Column<int>(type: "integer", nullable: false),
                    CompletenessScore = table.Column<int>(type: "integer", nullable: false),
                    TestabilityScore = table.Column<int>(type: "integer", nullable: false),
                    AcceptanceCriteriaScore = table.Column<int>(type: "integer", nullable: false),
                    ScopeScore = table.Column<int>(type: "integer", nullable: false),
                    BusinessValueScore = table.Column<int>(type: "integer", nullable: false),
                    AmbiguityScore = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    OverallScore = table.Column<int>(type: "integer", nullable: false),
                    Strengths = table.Column<string>(type: "text", nullable: false),
                    Weaknesses = table.Column<string>(type: "text", nullable: false),
                    Suggestions = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStoryQualityScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStoryQualityScores_UserStories_UserStoryId",
                        column: x => x.UserStoryId,
                        principalTable: "UserStories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonaQualityScores_PersonaId",
                table: "PersonaQualityScores",
                column: "PersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioQualityScores_ScenarioId",
                table: "ScenarioQualityScores",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStoryQualityScores_UserStoryId",
                table: "UserStoryQualityScores",
                column: "UserStoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonaQualityScores");

            migrationBuilder.DropTable(
                name: "ScenarioQualityScores");

            migrationBuilder.DropTable(
                name: "UserStoryQualityScores");
        }
    }
}
