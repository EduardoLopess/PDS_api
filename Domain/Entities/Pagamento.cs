using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities
{
    public class Pagamento
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }
        public double ValorPago { get; set; }
        public FormaPagamento FormaPagamento { get; set; }
        public DateTime DateTime { get; set; }
    }
}