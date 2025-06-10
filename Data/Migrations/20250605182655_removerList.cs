using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class removerList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drinks_Sabores_SaborId",
                table: "Drinks");

            migrationBuilder.DropIndex(
                name: "IX_Drinks_SaborId",
                table: "Drinks");

            migrationBuilder.AddColumn<int>(
                name: "DrinkId",
                table: "Sabores",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sabores_DrinkId",
                table: "Sabores",
                column: "DrinkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sabores_Drinks_DrinkId",
                table: "Sabores",
                column: "DrinkId",
                principalTable: "Drinks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sabores_Drinks_DrinkId",
                table: "Sabores");

            migrationBuilder.DropIndex(
                name: "IX_Sabores_DrinkId",
                table: "Sabores");

            migrationBuilder.DropColumn(
                name: "DrinkId",
                table: "Sabores");

            migrationBuilder.CreateIndex(
                name: "IX_Drinks_SaborId",
                table: "Drinks",
                column: "SaborId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drinks_Sabores_SaborId",
                table: "Drinks",
                column: "SaborId",
                principalTable: "Sabores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
