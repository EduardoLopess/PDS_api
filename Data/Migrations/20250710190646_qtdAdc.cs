using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class qtdAdc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdicionalItem");

            migrationBuilder.AddColumn<int>(
                name: "AdicionalId",
                table: "Itens",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Itens_AdicionalId",
                table: "Itens",
                column: "AdicionalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Itens_Adicionals_AdicionalId",
                table: "Itens",
                column: "AdicionalId",
                principalTable: "Adicionals",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Itens_Adicionals_AdicionalId",
                table: "Itens");

            migrationBuilder.DropIndex(
                name: "IX_Itens_AdicionalId",
                table: "Itens");

            migrationBuilder.DropColumn(
                name: "AdicionalId",
                table: "Itens");

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
    }
}
