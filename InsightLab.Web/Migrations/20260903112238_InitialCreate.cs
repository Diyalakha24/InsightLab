using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsightLab.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Experiments",
                columns: table => new
                {
                    ExperimentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExperimentName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiments", x => x.ExperimentId);
                });

            migrationBuilder.CreateTable(
                name: "ExperimentParticipants",
                columns: table => new
                {
                    ParticipantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExperimentId = table.Column<int>(type: "int", nullable: false),
                    Variant = table.Column<int>(type: "int", nullable: false),
                    SessionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Converted = table.Column<bool>(type: "bit", nullable: false),
                    OrderValue = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperimentParticipants", x => x.ParticipantId);
                    table.ForeignKey(
                        name: "FK_ExperimentParticipants_Experiments_ExperimentId",
                        column: x => x.ExperimentId,
                        principalTable: "Experiments",
                        principalColumn: "ExperimentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentParticipants_Converted",
                table: "ExperimentParticipants",
                column: "Converted");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentParticipants_ExperimentId",
                table: "ExperimentParticipants",
                column: "ExperimentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentParticipants_Variant",
                table: "ExperimentParticipants",
                column: "Variant");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExperimentParticipants");

            migrationBuilder.DropTable(
                name: "Experiments");
        }
    }
}
