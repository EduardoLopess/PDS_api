using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Service;

namespace Data.Validation
{
    public class MesaValidation : IMesaService
    {
        private readonly IMesaRepository _mesaRepository;
        public MesaValidation(IMesaRepository mesaRepository)
        {
            _mesaRepository = mesaRepository;
        }

        public async Task<bool> NumeroCadastrado(int numeroMesa)
        {
            return await _mesaRepository.ExisteMesaPorNumeroAsync(numeroMesa);
        }

        public async Task<bool> MesaExiste(int id)
        {
            var mesaExiste = await _mesaRepository.GetByIdAsync(id);
            if (mesaExiste == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> MesaOcupada(int id)
        {
            return await _mesaRepository.MesaOcupada(id);
        }

        public async Task<bool> MudaStatusMesaAsync(int id)
        {
            return await _mesaRepository.MudaStatusMesaAsync(id);
        }
    }
}