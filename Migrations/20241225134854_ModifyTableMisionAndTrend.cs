using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Meta_xi.Migrations
{
    /// <inheritdoc />
    public partial class ModifyTableMisionAndTrend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Completeds");

            migrationBuilder.DropColumn(
                name: "Goal",
                table: "Trends");

            migrationBuilder.DropColumn(
                name: "IsClaimed",
                table: "Trends");

            migrationBuilder.DropColumn(
                name: "Progres",
                table: "Trends");

            migrationBuilder.DropColumn(
                name: "Goal",
                table: "Missionss");

            migrationBuilder.DropColumn(
                name: "IsClaimed",
                table: "Missionss");

            migrationBuilder.DropColumn(
                name: "Progres",
                table: "Missionss");

            migrationBuilder.AddColumn<int>(
                name: "Goal",
                table: "MissionsUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsClaimed",
                table: "MissionsUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Progres",
                table: "MissionsUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TrendUser",
                columns: table => new
                {
                    IdTrendUser = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IDTrend = table.Column<int>(type: "integer", nullable: false),
                    IdUser = table.Column<int>(type: "integer", nullable: false),
                    Progres = table.Column<int>(type: "integer", nullable: false),
                    Goal = table.Column<int>(type: "integer", nullable: false),
                    IsClaimed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrendUser", x => x.IdTrendUser);
                    table.ForeignKey(
                        name: "FK_TrendUser_Trends_IDTrend",
                        column: x => x.IDTrend,
                        principalTable: "Trends",
                        principalColumn: "IdTendency",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrendUser_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrendUser_IDTrend",
                table: "TrendUser",
                column: "IDTrend");

            migrationBuilder.CreateIndex(
                name: "IX_TrendUser_IdUser",
                table: "TrendUser",
                column: "IdUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrendUser");

            migrationBuilder.DropColumn(
                name: "Goal",
                table: "MissionsUsers");

            migrationBuilder.DropColumn(
                name: "IsClaimed",
                table: "MissionsUsers");

            migrationBuilder.DropColumn(
                name: "Progres",
                table: "MissionsUsers");

            migrationBuilder.AddColumn<int>(
                name: "Goal",
                table: "Trends",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsClaimed",
                table: "Trends",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Progres",
                table: "Trends",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Goal",
                table: "Missionss",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsClaimed",
                table: "Missionss",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Progres",
                table: "Missionss",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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
        }
    }
}
