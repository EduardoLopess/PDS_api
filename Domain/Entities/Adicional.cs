namespace Domain.Entities
{
    public class Adicional
    {
        public int Id { get; set; }
        public string? AdicionalNome { get; set; }
        public double PrecoAdicional { get; set; }
        public bool DisponibilidadeAdicional  { get; set; }
        public List<Item> Itens { get; set; } 
    }
}