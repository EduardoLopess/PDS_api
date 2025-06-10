using Domain.Enums;

namespace Domain.DTOs
{
    public class ProdutoDTO
    {
        public int Id { get; set; }
        public string? NomeProduto { get; set; }
        public double PrecoProduto { get; set; }
        public string? PrecoProdutoFormatado { get; set; }
        public bool DisponibilidadeProduto { get; set; }
        public string? CategoriaProduto { get; set; }
        public string? TipoProduto { get; set; }
        
    }
}