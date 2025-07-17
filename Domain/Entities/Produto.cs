using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities
{
    public class Produto
    {
        public int Id { get; set; }
        public string? NomeProduto { get; set; }
        public double PrecoProduto { get; set; }
        public bool DisponibilidadeProduto { get; set; }
        public CategoriaProduto CategoriaProduto { get; set; }
        public TipoProduto TipoProduto { get; set; }
        public int CodigoNCM { get; set; }
        
    }
}