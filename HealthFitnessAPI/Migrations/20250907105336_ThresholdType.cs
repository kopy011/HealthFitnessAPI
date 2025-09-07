using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthFitnessAPI.Migrations
{
    /// <inheritdoc />
    public partial class ThresholdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "AchievementLevelThresholds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ThresholdType",
                table: "AchievementLevelThresholds",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "AchievementLevelThresholds");

            migrationBuilder.DropColumn(
                name: "ThresholdType",
                table: "AchievementLevelThresholds");
        }
    }
}
