using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class updateEntidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrinkSabor");

            migrationBuilder.AddColumn<string>(
                name: "NomeSabor",
                table: "Sabores",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeDrink",
                table: "Drinks",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SaborId",
                table: "Drinks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TipoDrink",
                table: "Drinks",
                type: "text",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drinks_Sabores_SaborId",
                table: "Drinks");

            migrationBuilder.DropIndex(
                name: "IX_Drinks_SaborId",
                table: "Drinks");

            migrationBuilder.DropColumn(
                name: "NomeSabor",
                table: "Sabores");

            migrationBuilder.DropColumn(
                name: "NomeDrink",
                table: "Drinks");

            migrationBuilder.DropColumn(
                name: "SaborId",
                table: "Drinks");

            migrationBuilder.DropColumn(
                name: "TipoDrink",
                table: "Drinks");

            migrationBuilder.CreateTable(
                name: "DrinkSabor",
                columns: table => new
                {
                    DrinksId = table.Column<int>(type: "integer", nullable: false),
                    SaboresId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrinkSabor", x => new { x.DrinksId, x.SaboresId });
                    table.ForeignKey(
                        name: "FK_DrinkSabor_Drinks_DrinksId",
                        column: x => x.DrinksId,
                        principalTable: "Drinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrinkSabor_Sabores_SaboresId",
                        column: x => x.SaboresId,
                        principalTable: "Sabores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrinkSabor_SaboresId",
                table: "DrinkSabor",
                column: "SaboresId");
        }
    }
}
