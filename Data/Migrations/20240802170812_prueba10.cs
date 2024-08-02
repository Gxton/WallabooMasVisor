using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallaboo.Data.Migrations
{
    /// <inheritdoc />
    public partial class prueba10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Ciudades_ProvinciaId",
                table: "Ciudades",
                column: "ProvinciaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ciudades_Provincias_ProvinciaId",
                table: "Ciudades",
                column: "ProvinciaId",
                principalTable: "Provincias",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ciudades_Provincias_ProvinciaId",
                table: "Ciudades");

            migrationBuilder.DropIndex(
                name: "IX_Ciudades_ProvinciaId",
                table: "Ciudades");
        }
    }
}
