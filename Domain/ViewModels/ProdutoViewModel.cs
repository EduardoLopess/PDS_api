using Domain.Enums;

namespace Domain.ViewModels
{
    public class ProdutoViewModel
    {
        public string? NomeProduto { get; set; }
        public double? PrecoProduto { get; set; }
        public bool DisponibilidadeProduto { get; set; }
        public CategoriaProduto? CategoriaProduto { get; set; }
        public TipoProduto? TipoProduto { get; set; }
        public int CodigoNCM { get; set; }

    }
}