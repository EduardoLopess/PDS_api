namespace Domain.DTOs
{
    public class PedidoDTO
    {
        public int Id { get; set; }
        // public MesaDTO? Mesa { get; set; }
        public int NumeroMesa { get;  set;}
        public DateTime DateTime { get; set; }
        public string DateTimeFormatado { get; set; }
        public double TotalPedido { get; set; }
        public IList<ItemDTO?> Itens { get; set; }
    }
}