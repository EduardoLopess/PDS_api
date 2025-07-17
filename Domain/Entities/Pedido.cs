namespace Domain.Entities
{
    public class Pedido
    {
        public int Id { get; set; }
        public Mesa? Mesa { get; set; }
        public DateTime DateTime { get; set; }
        public double TotalPedido { get; set; }
        public IList<Item?> Itens { get; set; }
        public bool StatusPedido { get; set; }
        public string? ConnectionId { get; set; }
        
    }
}