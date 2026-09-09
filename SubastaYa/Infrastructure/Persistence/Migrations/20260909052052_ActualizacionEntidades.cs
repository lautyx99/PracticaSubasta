using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ActualizacionEntidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Billeteras_UsuarioId",
                table: "Billeteras");

            migrationBuilder.AddColumn<int>(
                name: "BilleteraId1",
                table: "Transacciones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubastaId1",
                table: "Transacciones",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_BilleteraId1",
                table: "Transacciones",
                column: "BilleteraId1");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_SubastaId1",
                table: "Transacciones",
                column: "SubastaId1");

            migrationBuilder.CreateIndex(
                name: "IX_Billeteras_UsuarioId",
                table: "Billeteras",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transacciones_Billeteras_BilleteraId1",
                table: "Transacciones",
                column: "BilleteraId1",
                principalTable: "Billeteras",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacciones_Subastas_SubastaId1",
                table: "Transacciones",
                column: "SubastaId1",
                principalTable: "Subastas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transacciones_Billeteras_BilleteraId1",
                table: "Transacciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Transacciones_Subastas_SubastaId1",
                table: "Transacciones");

            migrationBuilder.DropIndex(
                name: "IX_Transacciones_BilleteraId1",
                table: "Transacciones");

            migrationBuilder.DropIndex(
                name: "IX_Transacciones_SubastaId1",
                table: "Transacciones");

            migrationBuilder.DropIndex(
                name: "IX_Billeteras_UsuarioId",
                table: "Billeteras");

            migrationBuilder.DropColumn(
                name: "BilleteraId1",
                table: "Transacciones");

            migrationBuilder.DropColumn(
                name: "SubastaId1",
                table: "Transacciones");

            migrationBuilder.CreateIndex(
                name: "IX_Billeteras_UsuarioId",
                table: "Billeteras",
                column: "UsuarioId");
        }
    }
}
