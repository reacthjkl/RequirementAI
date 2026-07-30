using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RequirementAI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class QualityAnalysisQueue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QualityAnalysisJobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ErrorMessage = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    FinishedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityAnalysisJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualityAnalysisJobs_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QualityAnalysisJobs_ProjectId",
                table: "QualityAnalysisJobs",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QualityAnalysisJobs");
        }
    }
}
