using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallaboo.Data.Migrations
{
    /// <inheritdoc />
    public partial class _02102024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Imagenes_AnuncioId",
                table: "Imagenes",
                column: "AnuncioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Imagenes_Anuncios_AnuncioId",
                table: "Imagenes",
                column: "AnuncioId",
                principalTable: "Anuncios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Imagenes_Anuncios_AnuncioId",
                table: "Imagenes");

            migrationBuilder.DropIndex(
                name: "IX_Imagenes_AnuncioId",
                table: "Imagenes");
        }
    }
}
