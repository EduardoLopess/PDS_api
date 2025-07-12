using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.InputModels;

namespace Domain.ViewModels
{
    public class AtualizarItemViewModel
    {
        public int ProdutoId { get; set; }
        public int Qtd { get; set; }
        public int? SaborDrinkId { get; set; }
        public IList<ItemAdicionalInputViewModel> Adicionais { get; set; }
       
    }
}