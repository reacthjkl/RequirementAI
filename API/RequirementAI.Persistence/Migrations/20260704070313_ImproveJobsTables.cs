using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RequirementAI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ImproveJobsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectRefinementJobs_Projects_ProjectId",
                table: "ProjectRefinementJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_QualityAnalysisJobs_Projects_ProjectId",
                table: "QualityAnalysisJobs");

            migrationBuilder.DropIndex(
                name: "IX_QualityAnalysisJobs_ProjectId",
                table: "QualityAnalysisJobs");

            migrationBuilder.DropIndex(
                name: "IX_ProjectRefinementJobs_ProjectId",
                table: "ProjectRefinementJobs");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "QualityAnalysisJobs");

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "QualityAnalysisJobs");

            migrationBuilder.DropColumn(
                name: "FinishedAt",
                table: "QualityAnalysisJobs");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "QualityAnalysisJobs");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "QualityAnalysisJobs");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "QualityAnalysisJobs");

            migrationBuilder.DropColumn(
                name: "TryCount",
                table: "QualityAnalysisJobs");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "QualityAnalysisJobs");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProjectRefinementJobs");

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "ProjectRefinementJobs");

            migrationBuilder.DropColumn(
                name: "FinishedAt",
                table: "ProjectRefinementJobs");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ProjectRefinementJobs");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "ProjectRefinementJobs");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProjectRefinementJobs");

            migrationBuilder.DropColumn(
                name: "TryCount",
                table: "ProjectRefinementJobs");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ProjectRefinementJobs");

            migrationBuilder.CreateTable(
                name: "BaseJobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ErrorMessage = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    FinishedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    FinishedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    TryCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseJobs_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaseJobs_ProjectId",
                table: "BaseJobs",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectRefinementJobs_BaseJobs_Id",
                table: "ProjectRefinementJobs",
                column: "Id",
                principalTable: "BaseJobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QualityAnalysisJobs_BaseJobs_Id",
                table: "QualityAnalysisJobs",
                column: "Id",
                principalTable: "BaseJobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectRefinementJobs_BaseJobs_Id",
                table: "ProjectRefinementJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_QualityAnalysisJobs_BaseJobs_Id",
                table: "QualityAnalysisJobs");

            migrationBuilder.DropTable(
                name: "BaseJobs");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "QualityAnalysisJobs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "QualityAnalysisJobs",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "FinishedAt",
                table: "QualityAnalysisJobs",
                type: "timestamptz",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "QualityAnalysisJobs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartedAt",
                table: "QualityAnalysisJobs",
                type: "timestamptz",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "QualityAnalysisJobs",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TryCount",
                table: "QualityAnalysisJobs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "QualityAnalysisJobs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "ProjectRefinementJobs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "ProjectRefinementJobs",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "FinishedAt",
                table: "ProjectRefinementJobs",
                type: "timestamptz",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "ProjectRefinementJobs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartedAt",
                table: "ProjectRefinementJobs",
                type: "timestamptz",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ProjectRefinementJobs",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TryCount",
                table: "ProjectRefinementJobs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "ProjectRefinementJobs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_QualityAnalysisJobs_ProjectId",
                table: "QualityAnalysisJobs",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRefinementJobs_ProjectId",
                table: "ProjectRefinementJobs",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectRefinementJobs_Projects_ProjectId",
                table: "ProjectRefinementJobs",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QualityAnalysisJobs_Projects_ProjectId",
                table: "QualityAnalysisJobs",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
