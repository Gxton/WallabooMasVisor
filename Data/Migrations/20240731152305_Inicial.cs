using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallaboo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblCliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreComercial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionComercial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelefonoComercial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    URLComercial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescripcionComercial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblPais",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombrePais = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPais", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tblAnuncio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAnuncio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblAnuncio_tblCliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "tblCliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblProvincia",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreProvincia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Paisid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblProvincia", x => x.id);
                    table.ForeignKey(
                        name: "FK_tblProvincia_tblPais_Paisid",
                        column: x => x.Paisid,
                        principalTable: "tblPais",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblCiudad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCiudad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Provinciaid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCiudad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblCiudad_tblProvincia_Provinciaid",
                        column: x => x.Provinciaid,
                        principalTable: "tblProvincia",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblAnuncio_ClienteId",
                table: "tblAnuncio",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_tblAnuncio_TenantId",
                table: "tblAnuncio",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_tblCiudad_Provinciaid",
                table: "tblCiudad",
                column: "Provinciaid");

            migrationBuilder.CreateIndex(
                name: "IX_tblCliente_TenantId",
                table: "tblCliente",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_tblProvincia_Paisid",
                table: "tblProvincia",
                column: "Paisid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblAnuncio");

            migrationBuilder.DropTable(
                name: "tblCiudad");

            migrationBuilder.DropTable(
                name: "tblCliente");

            migrationBuilder.DropTable(
                name: "tblProvincia");

            migrationBuilder.DropTable(
                name: "tblPais");
        }
    }
}
