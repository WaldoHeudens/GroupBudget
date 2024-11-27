using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupBudget_Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class Seeder_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StartedById",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_StartedById",
                table: "Projects",
                column: "StartedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_StartedById",
                table: "Projects",
                column: "StartedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_StartedById",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_StartedById",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "StartedById",
                table: "Projects");
        }
    }
}
