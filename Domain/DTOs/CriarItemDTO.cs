using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CriarItemDTO
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public int Qtd { get; set; }
    }
}