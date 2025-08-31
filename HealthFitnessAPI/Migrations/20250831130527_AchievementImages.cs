using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthFitnessAPI.Migrations
{
    /// <inheritdoc />
    public partial class AchievementImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoFilePath",
                table: "AchievementLevels");

            migrationBuilder.AddColumn<string>(
                name: "LogoPath",
                table: "AchievementLevelThresholds",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoPath",
                table: "AchievementLevelThresholds");

            migrationBuilder.AddColumn<string>(
                name: "LogoFilePath",
                table: "AchievementLevels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
