using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CriarPedidoDTO
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public List<CriarItemDTO> Itens { get; set; }
    }
}