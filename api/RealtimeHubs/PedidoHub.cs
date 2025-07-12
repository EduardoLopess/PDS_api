using Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace api.RealtimeHubs
{
    public class PedidoHub : Hub
    {
        public async Task AtualizarPedido(string mensagem)
        {
            await Clients.All.SendAsync("RecebendoAtualizacao", mensagem);
        }

        public async Task PedidoCriado(Pedido pedido)
        {
            await Clients.All.SendAsync("PedidoCriado", pedido);
        }

        public async Task PedidoCancelado(int pedidoId)
        {
            await Clients.All.SendAsync("PedidoCancelado", pedidoId);
        }

        public async Task NovaNotificacao(string mensagem)
        {
            await Clients.All.SendAsync("NovaNotificacao", mensagem);
        }

        public async Task PedidoStatusAlterado(Pedido pedido)
        {
            await Clients.All.SendAsync("PedidoStatusAlterado", pedido);
        }

        public async Task MesaPedidoAtualizada(Pedido pedido)
        {
            await Clients.All.SendAsync("MesaPedidoAtualizada", pedido);
        }

    }
}