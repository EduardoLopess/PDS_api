using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class AdicionalDTO
    {
        public int Id { get; set; }
        public string? AdicionalNome { get; set; }
        public double PrecoAdicional { get; set; }
        public string? PrecoAdicionalFormatado { get; set; }
        public bool DisponibilidadeAdicional { get; set; }
        public int Quantidade { get; set; }
    }
}