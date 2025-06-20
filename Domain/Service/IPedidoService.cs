
using Domain.Entities;
using Domain.ViewModels;

namespace Domain.Service
{
    public interface IPedidoService
    {
        Task<Pedido> CreatePedidoService(CriarPedidoViewModel viewModel);
        // Task<Pedido> AtualizarPedidoService(int pedidoId, CriarItemViewModel viewModel);
        Task<Pedido> AdicionarItemNovo(int pedidoId, CriarPedidoViewModel updateModel);
        Task<Pedido> RemoverItensAsync(int pedidoId, List<int> itemIds);

    }
}