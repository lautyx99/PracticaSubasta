using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixShadowKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transacciones_Billeteras_BilleteraId1",
                table: "Transacciones");

            migrationBuilder.DropIndex(
                name: "IX_Transacciones_BilleteraId1",
                table: "Transacciones");

            migrationBuilder.DropColumn(
                name: "BilleteraId1",
                table: "Transacciones");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BilleteraId1",
                table: "Transacciones",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_BilleteraId1",
                table: "Transacciones",
                column: "BilleteraId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacciones_Billeteras_BilleteraId1",
                table: "Transacciones",
                column: "BilleteraId1",
                principalTable: "Billeteras",
                principalColumn: "Id");
        }
    }
}
