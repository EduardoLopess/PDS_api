using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.DTOs
{
    public class DrinkDTO
    {
        public int Id { get; set; }
        public string? NomeDrink { get; set; }
        public TipoDrink TipoDrink { get; set; }
        public SaborDTO? Sabores { get; set; }
    }
}