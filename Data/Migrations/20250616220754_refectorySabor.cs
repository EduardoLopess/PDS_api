using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class refectorySabor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Disponivel",
                table: "Sabores",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SaborDrinkId",
                table: "Itens",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Itens_SaborDrinkId",
                table: "Itens",
                column: "SaborDrinkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Itens_Sabores_SaborDrinkId",
                table: "Itens",
                column: "SaborDrinkId",
                principalTable: "Sabores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Itens_Sabores_SaborDrinkId",
                table: "Itens");

            migrationBuilder.DropIndex(
                name: "IX_Itens_SaborDrinkId",
                table: "Itens");

            migrationBuilder.DropColumn(
                name: "Disponivel",
                table: "Sabores");

            migrationBuilder.DropColumn(
                name: "SaborDrinkId",
                table: "Itens");
        }
    }
}
