
using Domain.Entities;
using Domain.InputModels;
using Domain.ViewModels;

namespace Domain.Service
{
    public interface IPedidoService
    {
        Task<Pedido> CreatePedidoService(CriarPedidoViewModel viewModel);
        // Task<Pedido> AtualizarPedidoService(int pedidoId, CriarItemViewModel viewModel);
        Task<Pedido> AdicionarItemNovo(int pedidoId, CriarPedidoViewModel updateModel);
        Task<Pedido> RemoverItensAsync(int pedidoId, List<int> itemIds);
        Task<Pedido> RemoverAdicional(int pedidoId, RemoverAdicionalViewModel model);
        Task<Pedido> AtualizarPedidoCompleto(int pedidoId, AtualizarPedidoViewModel model);
        Task<bool> TentarBloquearPedidoAsync(int pedidoId, string connectionIdDoCliente);
        Task<bool> DesbloquearPedidoAsync(int pedidoId, string connectionIdDoCliente);
        Task<Pedido> AtualizarMesaPedido(int pedidoId, int mesaId);

    }
}