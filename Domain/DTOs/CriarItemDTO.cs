namespace Domain.DTOs
{
    public class CriarItemDTO
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public int Qtd { get; set; }
        public SaborDTO? SaborDrink { get; set; }
        public List<AdicionalDTO?> Adicionals { get; set; } // adicionais que vão aparecer na resposta
    }
}