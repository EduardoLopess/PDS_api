using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class mnc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Drinks_DrinkId",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_DrinkId",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "DrinkId",
                table: "Produtos");

            migrationBuilder.AddColumn<int>(
                name: "CodigoNCM",
                table: "Produtos",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoNCM",
                table: "Produtos");

            migrationBuilder.AddColumn<int>(
                name: "DrinkId",
                table: "Produtos",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_DrinkId",
                table: "Produtos",
                column: "DrinkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Drinks_DrinkId",
                table: "Produtos",
                column: "DrinkId",
                principalTable: "Drinks",
                principalColumn: "Id");
        }
    }
}
