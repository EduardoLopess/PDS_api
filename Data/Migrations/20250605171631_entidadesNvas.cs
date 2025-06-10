using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class entidadesNvas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DrinkId",
                table: "Produtos",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Drinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MesaId = table.Column<int>(type: "integer", nullable: true),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalPedido = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pedidos_Mesas_MesaId",
                        column: x => x.MesaId,
                        principalTable: "Mesas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Sabores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sabores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Itens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PedidoId = table.Column<int>(type: "integer", nullable: false),
                    ProdutoId = table.Column<int>(type: "integer", nullable: false),
                    Qtd = table.Column<int>(type: "integer", nullable: false),
                    PrecoUnitario = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Itens_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Itens_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pagamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PedidoId = table.Column<int>(type: "integer", nullable: false),
                    ValorPago = table.Column<double>(type: "double precision", nullable: false),
                    FormaPagamento = table.Column<int>(type: "integer", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagamentos_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_Produtos_DrinkId",
                table: "Produtos",
                column: "DrinkId");

            migrationBuilder.CreateIndex(
                name: "IX_DrinkSabor_SaboresId",
                table: "DrinkSabor",
                column: "SaboresId");

            migrationBuilder.CreateIndex(
                name: "IX_Itens_PedidoId",
                table: "Itens",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Itens_ProdutoId",
                table: "Itens",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_PedidoId",
                table: "Pagamentos",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_MesaId",
                table: "Pedidos",
                column: "MesaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Drinks_DrinkId",
                table: "Produtos",
                column: "DrinkId",
                principalTable: "Drinks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Drinks_DrinkId",
                table: "Produtos");

            migrationBuilder.DropTable(
                name: "DrinkSabor");

            migrationBuilder.DropTable(
                name: "Itens");

            migrationBuilder.DropTable(
                name: "Pagamentos");

            migrationBuilder.DropTable(
                name: "Drinks");

            migrationBuilder.DropTable(
                name: "Sabores");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_DrinkId",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "DrinkId",
                table: "Produtos");
        }
    }
}
