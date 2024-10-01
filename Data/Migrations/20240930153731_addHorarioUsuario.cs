using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallaboo.Data.Migrations
{
    /// <inheritdoc />
    public partial class addHorarioUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HorarioComercial",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HorarioComercial",
                table: "Usuarios");
        }
    }
}
