using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallaboo.Data.Migrations
{
    /// <inheritdoc />
    public partial class prueba7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.AddColumn<string>(
                name: "DescripcionComercial",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DireccionComercial",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreComercial",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelefonoComercial",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "URLComercial",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TenantId",
                table: "AspNetUsers",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TenantId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DescripcionComercial",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DireccionComercial",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NombreComercial",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TelefonoComercial",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "URLComercial",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    TenantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DescripcionComercial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionComercial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreComercial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelefonoComercial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    URLComercial = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.TenantId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_TenantId",
                table: "Clientes",
                column: "TenantId");
        }
    }
}
