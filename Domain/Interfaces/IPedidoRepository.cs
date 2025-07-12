using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPedidoRepository : IBaseRepository<Pedido>
    {
        Task CreatePedidoAsync(Pedido entity);
        Task<Pedido?> BuscarPedidoAtivoMesaAsync(int mesaId);
        Task<bool> TentarBloquearPedidoAsync(int pedidoId, string connectionIdDoCliente);
        Task<bool> DesbloquearPedidoAsync(int pedidoId, string connectionIdDoCliente);
        // Task<Pedido> AtualizarMesaPedido(int pedidoId, int mesaId);
    }
}