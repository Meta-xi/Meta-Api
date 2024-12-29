using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meta_xi.Migrations
{
    /// <inheritdoc />
    public partial class ModifyTrend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrendUser_Trends_IDTrend",
                table: "TrendUser");

            migrationBuilder.DropForeignKey(
                name: "FK_TrendUser_Users_IdUser",
                table: "TrendUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrendUser",
                table: "TrendUser");

            migrationBuilder.DropColumn(
                name: "Goal",
                table: "TrendUser");

            migrationBuilder.DropColumn(
                name: "Progres",
                table: "TrendUser");

            migrationBuilder.RenameTable(
                name: "TrendUser",
                newName: "TrendsUsers");

            migrationBuilder.RenameIndex(
                name: "IX_TrendUser_IdUser",
                table: "TrendsUsers",
                newName: "IX_TrendsUsers_IdUser");

            migrationBuilder.RenameIndex(
                name: "IX_TrendUser_IDTrend",
                table: "TrendsUsers",
                newName: "IX_TrendsUsers_IDTrend");

            migrationBuilder.AddColumn<int>(
                name: "Goal",
                table: "Trends",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrendsUsers",
                table: "TrendsUsers",
                column: "IdTrendUser");

            migrationBuilder.AddForeignKey(
                name: "FK_TrendsUsers_Trends_IDTrend",
                table: "TrendsUsers",
                column: "IDTrend",
                principalTable: "Trends",
                principalColumn: "IdTendency",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrendsUsers_Users_IdUser",
                table: "TrendsUsers",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrendsUsers_Trends_IDTrend",
                table: "TrendsUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_TrendsUsers_Users_IdUser",
                table: "TrendsUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrendsUsers",
                table: "TrendsUsers");

            migrationBuilder.DropColumn(
                name: "Goal",
                table: "Trends");

            migrationBuilder.RenameTable(
                name: "TrendsUsers",
                newName: "TrendUser");

            migrationBuilder.RenameIndex(
                name: "IX_TrendsUsers_IdUser",
                table: "TrendUser",
                newName: "IX_TrendUser_IdUser");

            migrationBuilder.RenameIndex(
                name: "IX_TrendsUsers_IDTrend",
                table: "TrendUser",
                newName: "IX_TrendUser_IDTrend");

            migrationBuilder.AddColumn<int>(
                name: "Goal",
                table: "TrendUser",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Progres",
                table: "TrendUser",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrendUser",
                table: "TrendUser",
                column: "IdTrendUser");

            migrationBuilder.AddForeignKey(
                name: "FK_TrendUser_Trends_IDTrend",
                table: "TrendUser",
                column: "IDTrend",
                principalTable: "Trends",
                principalColumn: "IdTendency",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrendUser_Users_IdUser",
                table: "TrendUser",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
