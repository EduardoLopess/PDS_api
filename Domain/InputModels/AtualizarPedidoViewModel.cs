using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.ViewModels;

namespace Domain.InputModels
{
    public class AtualizarPedidoViewModel
    {
        public int MesaId { get; set; }
        public List<AtualizarItemViewModel> Itens { get; set; }
        public double TotalPedido { get; set; }
        public string? TokenBloqueio { get; set; }
        public bool StatusPedido { get; set; }
        
    }
}