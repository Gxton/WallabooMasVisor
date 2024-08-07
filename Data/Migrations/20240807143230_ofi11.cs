using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallaboo.Data.Migrations
{
    /// <inheritdoc />
    public partial class ofi11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "activo",
                table: "Anuncios",
                newName: "Activo");

            migrationBuilder.AddColumn<int>(
                name: "CantidadDias",
                table: "Anuncios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Pagado",
                table: "Anuncios",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CantidadDias",
                table: "Anuncios");

            migrationBuilder.DropColumn(
                name: "Pagado",
                table: "Anuncios");

            migrationBuilder.RenameColumn(
                name: "Activo",
                table: "Anuncios",
                newName: "activo");
        }
    }
}
