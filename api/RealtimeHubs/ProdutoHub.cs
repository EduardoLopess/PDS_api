using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace api.RealtimeHubs
{
    public class ProdutoHub : Hub
    {
        public async Task AtualizarProduto(string mensagem)
        {
            await Clients.All.SendAsync("RecebendoAtualizacao", mensagem);
        }

        public async Task NovaNotificacao(string mensagem)
        {
            await Clients.All.SendAsync("NovaNotificacao", mensagem);
        }

        public async Task ProdutoCriado(Produto produto)
        {
            await Clients.All.SendAsync("ProdutoCriado", produto);
        }
    }
}