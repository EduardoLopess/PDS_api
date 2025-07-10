using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class CriarPedidoViewModel
    {
        public int MesaId { get; set; }
        public DateTime DateTime { get; set; }
        public List<CriarItemViewModel> Itens { get; set; }
        public double TotalPedido { get; set; }
    }
}