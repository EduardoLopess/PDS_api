using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class fkSabor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Itens_Sabores_SaborDrinkId",
                table: "Itens");

            migrationBuilder.AddForeignKey(
                name: "FK_Itens_Sabores_SaborDrinkId",
                table: "Itens",
                column: "SaborDrinkId",
                principalTable: "Sabores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Itens_Sabores_SaborDrinkId",
                table: "Itens");

            migrationBuilder.AddForeignKey(
                name: "FK_Itens_Sabores_SaborDrinkId",
                table: "Itens",
                column: "SaborDrinkId",
                principalTable: "Sabores",
                principalColumn: "Id");
        }
    }
}
