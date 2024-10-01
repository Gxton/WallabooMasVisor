using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallaboo.Data.Migrations
{
    /// <inheritdoc />
    public partial class addQR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "QRURL",
                table: "Usuarios",
                type: "VARBINARY(MAX)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QRURL",
                table: "Usuarios");
        }
    }
}
