using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthFitnessAPI.Migrations
{
    /// <inheritdoc />
    public partial class AchievementLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AchievementLevel",
                table: "UserAchievements",
                newName: "AchievementLevelId");

            migrationBuilder.CreateTable(
                name: "AchievementLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LogoFilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AchievementLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AchievementLevelThresholds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FemaleThreshold = table.Column<int>(type: "int", nullable: false),
                    MaleThreshold = table.Column<int>(type: "int", nullable: false),
                    AchievementLevelId = table.Column<int>(type: "int", nullable: false),
                    AchievementId = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AchievementLevelThresholds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AchievementLevelThresholds_AchievementLevels_AchievementLevelId",
                        column: x => x.AchievementLevelId,
                        principalTable: "AchievementLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AchievementLevelThresholds_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalTable: "Achievements",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAchievements_AchievementLevelId",
                table: "UserAchievements",
                column: "AchievementLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_AchievementLevels_Name",
                table: "AchievementLevels",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AchievementLevelThresholds_AchievementId",
                table: "AchievementLevelThresholds",
                column: "AchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_AchievementLevelThresholds_AchievementLevelId",
                table: "AchievementLevelThresholds",
                column: "AchievementLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAchievements_AchievementLevels_AchievementLevelId",
                table: "UserAchievements",
                column: "AchievementLevelId",
                principalTable: "AchievementLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAchievements_AchievementLevels_AchievementLevelId",
                table: "UserAchievements");

            migrationBuilder.DropTable(
                name: "AchievementLevelThresholds");

            migrationBuilder.DropTable(
                name: "AchievementLevels");

            migrationBuilder.DropIndex(
                name: "IX_UserAchievements_AchievementLevelId",
                table: "UserAchievements");

            migrationBuilder.RenameColumn(
                name: "AchievementLevelId",
                table: "UserAchievements",
                newName: "AchievementLevel");
        }
    }
}
