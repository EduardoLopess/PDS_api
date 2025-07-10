using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class ItemAdicionalViewModel
    {
        public int AdicionalId { get; set; }
        public string Nome { get; set; }
        public int Qtd { get; set; }
        public double Preco { get; set; }
    }
}