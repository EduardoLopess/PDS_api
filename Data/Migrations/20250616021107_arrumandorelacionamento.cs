using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class arrumandorelacionamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "AdicionalItem",
                columns: table => new
                {
                    AdicionalsId = table.Column<int>(type: "integer", nullable: false),
                    ItensId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdicionalItem", x => new { x.AdicionalsId, x.ItensId });
                    table.ForeignKey(
                        name: "FK_AdicionalItem_Adicionals_AdicionalsId",
                        column: x => x.AdicionalsId,
                        principalTable: "Adicionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdicionalItem_Itens_ItensId",
                        column: x => x.ItensId,
                        principalTable: "Itens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdicionalItem_ItensId",
                table: "AdicionalItem",
                column: "ItensId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdicionalItem");

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Adicionals",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Adicionals_ItemId",
                table: "Adicionals",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adicionals_Itens_ItemId",
                table: "Adicionals",
                column: "ItemId",
                principalTable: "Itens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
