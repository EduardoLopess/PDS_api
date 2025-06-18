using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class relacionament : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adicionals_Itens_ItemId",
                table: "Adicionals");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "Adicionals",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Adicionals_Itens_ItemId",
                table: "Adicionals",
                column: "ItemId",
                principalTable: "Itens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adicionals_Itens_ItemId",
                table: "Adicionals");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "Adicionals",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Adicionals_Itens_ItemId",
                table: "Adicionals",
                column: "ItemId",
                principalTable: "Itens",
                principalColumn: "Id");
        }
    }
}
