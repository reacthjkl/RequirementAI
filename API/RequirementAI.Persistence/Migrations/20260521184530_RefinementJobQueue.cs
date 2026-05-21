using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RequirementAI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RefinementJobQueue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "UserStories",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1028)",
                oldMaxLength: 1028);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Scenarios",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1028)",
                oldMaxLength: 1028);

            migrationBuilder.AlterColumn<string>(
                name: "TriggerAction",
                table: "EdgeCases",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1028)",
                oldMaxLength: 1028);

            migrationBuilder.AlterColumn<string>(
                name: "Preconditions",
                table: "EdgeCases",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1028)",
                oldMaxLength: 1028);

            migrationBuilder.AlterColumn<string>(
                name: "ExpectedBehavior",
                table: "EdgeCases",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1028)",
                oldMaxLength: 1028);

            migrationBuilder.AlterColumn<string>(
                name: "Wording",
                table: "AcceptanceCriteria",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1028)",
                oldMaxLength: 1028);

            migrationBuilder.CreateTable(
                name: "ProjectRefinementJobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ErrorMessage = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    FinishedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRefinementJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectRefinementJobs_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRefinementJobs_ProjectId",
                table: "ProjectRefinementJobs",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectRefinementJobs");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "UserStories",
                type: "character varying(1028)",
                maxLength: 1028,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1024)",
                oldMaxLength: 1024);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Scenarios",
                type: "character varying(1028)",
                maxLength: 1028,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1024)",
                oldMaxLength: 1024);

            migrationBuilder.AlterColumn<string>(
                name: "TriggerAction",
                table: "EdgeCases",
                type: "character varying(1028)",
                maxLength: 1028,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1024)",
                oldMaxLength: 1024);

            migrationBuilder.AlterColumn<string>(
                name: "Preconditions",
                table: "EdgeCases",
                type: "character varying(1028)",
                maxLength: 1028,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1024)",
                oldMaxLength: 1024);

            migrationBuilder.AlterColumn<string>(
                name: "ExpectedBehavior",
                table: "EdgeCases",
                type: "character varying(1028)",
                maxLength: 1028,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1024)",
                oldMaxLength: 1024);

            migrationBuilder.AlterColumn<string>(
                name: "Wording",
                table: "AcceptanceCriteria",
                type: "character varying(1028)",
                maxLength: 1028,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1024)",
                oldMaxLength: 1024);
        }
    }
}
