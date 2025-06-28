using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.ViewModels;

namespace Domain.Service
{
    public interface IMesaService
    {
        Task<Mesa> CreateMesaService(MesaViewModel model);
        Task<bool> NumeroCadastrado(int numeroMesa);
        Task<bool> MesaExiste(int id);
        Task<bool> MesaOcupada(int id);
        Task<bool> MudaStatusMesaAsync(int id);
    }
}