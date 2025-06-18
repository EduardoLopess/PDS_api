using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class SaborDTO
    {
        public int Id { get; set; }
        public string? NomeSabor { get; set; }
        public bool Disponivel { get; set; }
    }
}