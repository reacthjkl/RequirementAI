using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RequirementAI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class reworkArtefactsStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserStories_Projects_ProjectId",
                table: "UserStories");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "UserStories",
                newName: "ScenarioId");

            migrationBuilder.RenameIndex(
                name: "IX_UserStories_ProjectId",
                table: "UserStories",
                newName: "IX_UserStories_ScenarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStories_Scenarios_ScenarioId",
                table: "UserStories",
                column: "ScenarioId",
                principalTable: "Scenarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserStories_Scenarios_ScenarioId",
                table: "UserStories");

            migrationBuilder.RenameColumn(
                name: "ScenarioId",
                table: "UserStories",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_UserStories_ScenarioId",
                table: "UserStories",
                newName: "IX_UserStories_ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStories_Projects_ProjectId",
                table: "UserStories",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
