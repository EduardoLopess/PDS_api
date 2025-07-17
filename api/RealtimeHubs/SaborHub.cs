using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace api.RealtimeHubs
{
    public class SaborHub : Hub
    {
        public async Task AtualizarSabor(string mensagem)
        {
            await Clients.All.SendAsync("RecebendoAtualizacao", mensagem);
        }

        public async Task SaborCriado(Sabor sabor)
        {
            await Clients.All.SendAsync("SaborCriado", sabor);
        }

    
    }
}