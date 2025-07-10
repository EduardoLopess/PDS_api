using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ItemAdicional
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int AdicionalId { get; set; }
        public Adicional Adicional { get; set; }

        public int Qtd { get; set; }
    }
}