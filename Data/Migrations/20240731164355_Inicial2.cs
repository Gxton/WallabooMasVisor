using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallaboo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Inicial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblAnuncio_tblCliente_ClienteId",
                table: "tblAnuncio");

            migrationBuilder.DropForeignKey(
                name: "FK_tblCiudad_tblProvincia_Provinciaid",
                table: "tblCiudad");

            migrationBuilder.DropForeignKey(
                name: "FK_tblProvincia_tblPais_Paisid",
                table: "tblProvincia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblProvincia",
                table: "tblProvincia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblPais",
                table: "tblPais");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblCliente",
                table: "tblCliente");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblCiudad",
                table: "tblCiudad");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblAnuncio",
                table: "tblAnuncio");

            migrationBuilder.RenameTable(
                name: "tblProvincia",
                newName: "Provincias");

            migrationBuilder.RenameTable(
                name: "tblPais",
                newName: "Paises");

            migrationBuilder.RenameTable(
                name: "tblCliente",
                newName: "Clientes");

            migrationBuilder.RenameTable(
                name: "tblCiudad",
                newName: "Ciudades");

            migrationBuilder.RenameTable(
                name: "tblAnuncio",
                newName: "Anuncios");

            migrationBuilder.RenameColumn(
                name: "Paisid",
                table: "Provincias",
                newName: "PaisId");

            migrationBuilder.RenameIndex(
                name: "IX_tblProvincia_Paisid",
                table: "Provincias",
                newName: "IX_Provincias_PaisId");

            migrationBuilder.RenameIndex(
                name: "IX_tblCliente_TenantId",
                table: "Clientes",
                newName: "IX_Clientes_TenantId");

            migrationBuilder.RenameColumn(
                name: "Provinciaid",
                table: "Ciudades",
                newName: "ProvinciaId");

            migrationBuilder.RenameIndex(
                name: "IX_tblCiudad_Provinciaid",
                table: "Ciudades",
                newName: "IX_Ciudades_ProvinciaId");

            migrationBuilder.RenameIndex(
                name: "IX_tblAnuncio_TenantId",
                table: "Anuncios",
                newName: "IX_Anuncios_TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_tblAnuncio_ClienteId",
                table: "Anuncios",
                newName: "IX_Anuncios_ClienteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Provincias",
                table: "Provincias",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Paises",
                table: "Paises",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clientes",
                table: "Clientes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ciudades",
                table: "Ciudades",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Anuncios",
                table: "Anuncios",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Paises",
                columns: new[] { "id", "NombrePais" },
                values: new object[] { 1, "Argentina" });

            migrationBuilder.InsertData(
                table: "Provincias",
                columns: new[] { "id", "NombreProvincia", "PaisId" },
                values: new object[] { 1, "Buenos Aires", 1 });

            migrationBuilder.InsertData(
                table: "Ciudades",
                columns: new[] { "Id", "NombreCiudad", "ProvinciaId" },
                values: new object[] { 1, "Mar del Plata", 1 });

            migrationBuilder.AddForeignKey(
                name: "FK_Anuncios_Clientes_ClienteId",
                table: "Anuncios",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ciudades_Provincias_ProvinciaId",
                table: "Ciudades",
                column: "ProvinciaId",
                principalTable: "Provincias",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Provincias_Paises_PaisId",
                table: "Provincias",
                column: "PaisId",
                principalTable: "Paises",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anuncios_Clientes_ClienteId",
                table: "Anuncios");

            migrationBuilder.DropForeignKey(
                name: "FK_Ciudades_Provincias_ProvinciaId",
                table: "Ciudades");

            migrationBuilder.DropForeignKey(
                name: "FK_Provincias_Paises_PaisId",
                table: "Provincias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Provincias",
                table: "Provincias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Paises",
                table: "Paises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clientes",
                table: "Clientes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ciudades",
                table: "Ciudades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Anuncios",
                table: "Anuncios");

            migrationBuilder.DeleteData(
                table: "Ciudades",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Provincias",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.RenameTable(
                name: "Provincias",
                newName: "tblProvincia");

            migrationBuilder.RenameTable(
                name: "Paises",
                newName: "tblPais");

            migrationBuilder.RenameTable(
                name: "Clientes",
                newName: "tblCliente");

            migrationBuilder.RenameTable(
                name: "Ciudades",
                newName: "tblCiudad");

            migrationBuilder.RenameTable(
                name: "Anuncios",
                newName: "tblAnuncio");

            migrationBuilder.RenameColumn(
                name: "PaisId",
                table: "tblProvincia",
                newName: "Paisid");

            migrationBuilder.RenameIndex(
                name: "IX_Provincias_PaisId",
                table: "tblProvincia",
                newName: "IX_tblProvincia_Paisid");

            migrationBuilder.RenameIndex(
                name: "IX_Clientes_TenantId",
                table: "tblCliente",
                newName: "IX_tblCliente_TenantId");

            migrationBuilder.RenameColumn(
                name: "ProvinciaId",
                table: "tblCiudad",
                newName: "Provinciaid");

            migrationBuilder.RenameIndex(
                name: "IX_Ciudades_ProvinciaId",
                table: "tblCiudad",
                newName: "IX_tblCiudad_Provinciaid");

            migrationBuilder.RenameIndex(
                name: "IX_Anuncios_TenantId",
                table: "tblAnuncio",
                newName: "IX_tblAnuncio_TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_Anuncios_ClienteId",
                table: "tblAnuncio",
                newName: "IX_tblAnuncio_ClienteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblProvincia",
                table: "tblProvincia",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblPais",
                table: "tblPais",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblCliente",
                table: "tblCliente",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblCiudad",
                table: "tblCiudad",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblAnuncio",
                table: "tblAnuncio",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblAnuncio_tblCliente_ClienteId",
                table: "tblAnuncio",
                column: "ClienteId",
                principalTable: "tblCliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tblCiudad_tblProvincia_Provinciaid",
                table: "tblCiudad",
                column: "Provinciaid",
                principalTable: "tblProvincia",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tblProvincia_tblPais_Paisid",
                table: "tblProvincia",
                column: "Paisid",
                principalTable: "tblPais",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
