using System;
using Domain.Enums;

namespace Domain.ViewModels
{
    public class DrinkViewModel
    {
       
        public string? DrinkNome { get; set; }
        public TipoDrink TipoDrink { get; set; }
        //dpublic SaborViewModel? Sabor { get; set; }
    }
}   