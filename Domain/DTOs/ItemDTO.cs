namespace Domain.DTOs
{
    public class ItemDTO
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public ProdutoDTO? Produto { get; set; }
        public int Qtd { get; set; }
        public double PrecoUnitario { get; set; }
    }
}