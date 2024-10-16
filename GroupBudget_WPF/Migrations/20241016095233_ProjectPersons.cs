using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupBudget_WPF.Migrations
{
    /// <inheritdoc />
    public partial class ProjectPersons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectPersonsIds",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.CreateIndex(
                name: "IX_PersonsProjects_ProjectId",
                table: "PersonsProjects",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonsProjects_Projects_ProjectId",
                table: "PersonsProjects",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonsProjects_Projects_ProjectId",
                table: "PersonsProjects");

            migrationBuilder.DropIndex(
                name: "IX_PersonsProjects_ProjectId",
                table: "PersonsProjects");

            migrationBuilder.DropColumn(
                name: "ProjectPersonsIds",
                table: "Projects");
        }
    }
}
