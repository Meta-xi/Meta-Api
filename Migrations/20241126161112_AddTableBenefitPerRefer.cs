using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Meta_xi.Migrations
{
    /// <inheritdoc />
    public partial class AddTableBenefitPerRefer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BenefitPerRefers",
                columns: table => new
                {
                    IdGanancias = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nivel1 = table.Column<double>(type: "double precision", nullable: false),
                    Nivel2 = table.Column<double>(type: "double precision", nullable: false),
                    Nivel3 = table.Column<double>(type: "double precision", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenefitPerRefers", x => x.IdGanancias);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BenefitPerRefers");
        }
    }
}
