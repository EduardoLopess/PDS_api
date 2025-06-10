using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class AdicionalDTO
    {
        public string? AdicionalNome { get; set; }
        public double PrecoAdicional { get; set; }
        public string? PrecoAdicionalFormatado { get; set; }
    }
}