using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Item
    {
        public int Id { get; set; }

        public int PedidoId { get; set; }
        public Pedido? Pedido { get; set; }
        public int ProdutoId { get; set; }
        public Produto? Produto { get; set; }
        public int Qtd { get; set; }
        public double PrecoUnitario { get; set; }
        public int? SaborDrinkId { get; set; } 

        public Sabor? SaborDrink { get; set; }
        public IList<ItemAdicional?> ItemAdicionais { get; set; } = new List<ItemAdicional?>();

    }
}