using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class listAdicional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Adicionals",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Adicionals_ItemId",
                table: "Adicionals",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adicionals_Itens_ItemId",
                table: "Adicionals",
                column: "ItemId",
                principalTable: "Itens",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adicionals_Itens_ItemId",
                table: "Adicionals");

            migrationBuilder.DropIndex(
                name: "IX_Adicionals_ItemId",
                table: "Adicionals");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Adicionals");
        }
    }
}
