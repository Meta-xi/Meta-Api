using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Meta_xi.Migrations
{
    /// <inheritdoc />
    public partial class TableWelcomeBonus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Completeds",
                columns: table => new
                {
                    IDFinished = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Completeds", x => x.IDFinished);
                });

            migrationBuilder.CreateTable(
                name: "Missionss",
                columns: table => new
                {
                    IDMission = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Progres = table.Column<int>(type: "integer", nullable: false),
                    Goal = table.Column<int>(type: "integer", nullable: false),
                    IsClaimed = table.Column<bool>(type: "boolean", nullable: false),
                    Reward = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Missionss", x => x.IDMission);
                });

            migrationBuilder.CreateTable(
                name: "Trends",
                columns: table => new
                {
                    IdTendency = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Progres = table.Column<int>(type: "integer", nullable: false),
                    Goal = table.Column<int>(type: "integer", nullable: false),
                    IsClaimed = table.Column<bool>(type: "boolean", nullable: false),
                    Reward = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trends", x => x.IdTendency);
                });

            migrationBuilder.CreateTable(
                name: "WelcomeBonuss",
                columns: table => new
                {
                    IDBonus = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserID = table.Column<int>(type: "integer", nullable: false),
                    IsClaimed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WelcomeBonuss", x => x.IDBonus);
                    table.ForeignKey(
                        name: "FK_WelcomeBonuss_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MissionsUsers",
                columns: table => new
                {
                    IDMissionUSer = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserID = table.Column<int>(type: "integer", nullable: false),
                    IDMission = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissionsUsers", x => x.IDMissionUSer);
                    table.ForeignKey(
                        name: "FK_MissionsUsers_Missionss_IDMission",
                        column: x => x.IDMission,
                        principalTable: "Missionss",
                        principalColumn: "IDMission",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MissionsUsers_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MissionsUsers_IDMission",
                table: "MissionsUsers",
                column: "IDMission");

            migrationBuilder.CreateIndex(
                name: "IX_MissionsUsers_UserID",
                table: "MissionsUsers",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_WelcomeBonuss_UserID",
                table: "WelcomeBonuss",
                column: "UserID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Completeds");

            migrationBuilder.DropTable(
                name: "MissionsUsers");

            migrationBuilder.DropTable(
                name: "Trends");

            migrationBuilder.DropTable(
                name: "WelcomeBonuss");

            migrationBuilder.DropTable(
                name: "Missionss");
        }
    }
}
