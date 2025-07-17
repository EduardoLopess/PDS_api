
using Domain.Enums;

namespace Domain.Entities
{
    public class Drink
    {
        public int Id { get; set; }
        public string? NomeDrink { get; set; }
        public TipoDrink TipoDrink { get; set; }
        public int SaborId { get; set; }
        public ICollection<Sabor> Sabores { get; set; } = [];
        
    }
}