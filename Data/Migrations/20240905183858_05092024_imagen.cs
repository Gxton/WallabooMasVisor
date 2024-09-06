using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallaboo.Data.Migrations
{
    /// <inheritdoc />
    public partial class _05092024_imagen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Imagenes",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Image1Path",
                table: "Imagenes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image2Path",
                table: "Imagenes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image3Path",
                table: "Imagenes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image4Path",
                table: "Imagenes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image5Path",
                table: "Imagenes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image1Path",
                table: "Imagenes");

            migrationBuilder.DropColumn(
                name: "Image2Path",
                table: "Imagenes");

            migrationBuilder.DropColumn(
                name: "Image3Path",
                table: "Imagenes");

            migrationBuilder.DropColumn(
                name: "Image4Path",
                table: "Imagenes");

            migrationBuilder.DropColumn(
                name: "Image5Path",
                table: "Imagenes");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Imagenes",
                newName: "ID");
        }
    }
}
