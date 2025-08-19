using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthFitnessAPI.Migrations
{
    /// <inheritdoc />
    public partial class AchievementCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AchievementType",
                table: "Achievements");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Achievements");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Achievements",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_Category",
                table: "Achievements",
                column: "Category",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Achievements_Category",
                table: "Achievements");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Achievements");

            migrationBuilder.AddColumn<int>(
                name: "AchievementType",
                table: "Achievements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Achievements",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
