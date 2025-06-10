using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Sabor
    {
        public int Id { get; set; }
        public string? NomeSabor { get; set; }
        //public IList<Drink> Drinks { get; set; } 
    }
}