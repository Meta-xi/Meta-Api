using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meta_xi.Migrations
{
    /// <inheritdoc />
    public partial class ModifyTableMissionUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Goal",
                table: "MissionsUsers");

            migrationBuilder.DropColumn(
                name: "Progres",
                table: "MissionsUsers");

            migrationBuilder.AddColumn<int>(
                name: "Goal",
                table: "Missionss",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Goal",
                table: "Missionss");

            migrationBuilder.AddColumn<int>(
                name: "Goal",
                table: "MissionsUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Progres",
                table: "MissionsUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
