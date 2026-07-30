using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RequirementAI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOverallScores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OverallScore",
                table: "UserStoryQualityScores");

            migrationBuilder.DropColumn(
                name: "OverallScore",
                table: "ScenarioQualityScores");

            migrationBuilder.DropColumn(
                name: "OverallScore",
                table: "PersonaQualityScores");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OverallScore",
                table: "UserStoryQualityScores",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverallScore",
                table: "ScenarioQualityScores",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverallScore",
                table: "PersonaQualityScores",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
