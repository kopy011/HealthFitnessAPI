using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthFitnessAPI.Migrations
{
    /// <inheritdoc />
    public partial class UserAchievementLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAchievementLikes",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserAchievementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAchievementLikes", x => new { x.UserId, x.UserAchievementId });
                    table.ForeignKey(
                        name: "FK_UserAchievementLikes_UserAchievements_UserAchievementId",
                        column: x => x.UserAchievementId,
                        principalTable: "UserAchievements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAchievementLikes_UserAchievementId",
                table: "UserAchievementLikes",
                column: "UserAchievementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAchievementLikes");
        }
    }
}
