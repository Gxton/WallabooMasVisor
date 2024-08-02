using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallaboo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Prueba2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anuncios_Clientes_ClienteId",
                table: "Anuncios");

            migrationBuilder.DropIndex(
                name: "IX_Anuncios_ClienteId",
                table: "Anuncios");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "Anuncios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClienteId",
                table: "Anuncios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Anuncios_ClienteId",
                table: "Anuncios",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Anuncios_Clientes_ClienteId",
                table: "Anuncios",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
