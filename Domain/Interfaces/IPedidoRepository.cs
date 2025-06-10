using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPedidoRepository : IBaseRepository<Pedido>
    {
        Task CreatePedidoAsync(Pedido entity, IList<int> ItemIds);
    }
}