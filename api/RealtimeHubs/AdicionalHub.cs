using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Migrations;
using Microsoft.AspNetCore.SignalR;

namespace api.RealtimeHubs
{
    public class AdicionalHub : Hub
    {
        public async Task AtualizarAdicional(string mensagem)
        {
            await Clients.All.SendAsync("RecebendoAtualizacao", mensagem);
        }

        public async Task AdicionalCriado(Adicional adicional)
        {
            await Clients.All.SendAsync("AdicionalCriado", adicional);
        }
        
    }
}