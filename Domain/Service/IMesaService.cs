using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Service
{
    public interface IMesaService
    {

        Task<bool> NumeroCadastrado(int numeroMesa);
        Task<bool> MesaExiste(int id);
        Task<bool> MesaOcupada(int id);
        Task<bool> MudaStatusMesaAsync(int id);
    }
}