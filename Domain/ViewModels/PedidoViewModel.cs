namespace Domain.ViewModels
{
    public class PedidoViewModel
    {
        public int Id { get; set; }
        public IList<ItemViewModel> Itens { get; set; }
        public MesaViewModel Mesa { get; set; }
        public DateTime DateTime { get; set; }
        public double TotalPedido { get; set; }

    }
}