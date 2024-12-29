using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Meta_xi.Migrations
{
    /// <inheritdoc />
    public partial class AddTwoTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DisponibilityToClaims",
                columns: table => new
                {
                    IDDisponibility = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserID = table.Column<int>(type: "integer", nullable: false),
                    Disponibility = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisponibilityToClaims", x => x.IDDisponibility);
                    table.ForeignKey(
                        name: "FK_DisponibilityToClaims_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IsClaimeds",
                columns: table => new
                {
                    IDClaimed = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IDMission = table.Column<int>(type: "integer", nullable: false),
                    UserID = table.Column<int>(type: "integer", nullable: false),
                    DateClaimed = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IsClaimeds", x => x.IDClaimed);
                    table.ForeignKey(
                        name: "FK_IsClaimeds_Missionss_IDMission",
                        column: x => x.IDMission,
                        principalTable: "Missionss",
                        principalColumn: "IDMission",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IsClaimeds_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DisponibilityToClaims_UserID",
                table: "DisponibilityToClaims",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_IsClaimeds_IDMission",
                table: "IsClaimeds",
                column: "IDMission");

            migrationBuilder.CreateIndex(
                name: "IX_IsClaimeds_UserID",
                table: "IsClaimeds",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisponibilityToClaims");

            migrationBuilder.DropTable(
                name: "IsClaimeds");
        }
    }
}
