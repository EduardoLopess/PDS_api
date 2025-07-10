namespace Domain.ViewModels
{
    public class ItemViewModel
    {
        public int ProdutoId { get; set; }
        public ProdutoViewModel? Produto { get; set; }
        public int Qtd { get; set; }
        public double PrecoUnitario { get; set; }
        // public IList<AdicionalViewModel?> Adicionais { get; set; }
        public List<ItemAdicionalViewModel> Adicionais { get; set; }
    }
}