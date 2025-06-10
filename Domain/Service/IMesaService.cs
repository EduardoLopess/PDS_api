using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Service
{
    public interface IMesaService
    {
        Task CriarMesa(Mesa mesa);
        Task AtualizarMesa(Mesa mesa);
        Task DeletarMesa(int id);
        Task<Mesa> GetById(int id);
        Task<IList<Mesa>> GetAll();

        Task<bool> MesaOcupada(int id);
        Task NumeroCadastrado(int id);
    }
}