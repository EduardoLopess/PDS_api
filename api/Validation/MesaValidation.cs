using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Service;
using Domain.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Data.Validation
{
    public class MesaValidation : IMesaService
    {
        private readonly IMesaRepository _mesaRepository;
        private readonly IMapper _mapper;
        public MesaValidation(IMesaRepository mesaRepository, IMapper mapper)
        {
            _mesaRepository = mesaRepository;
            _mapper = mapper;
        }

        public async Task<bool> NumeroCadastrado(int numeroMesa)
        {
            return await _mesaRepository.ExisteMesaPorNumeroAsync(numeroMesa);
        }

        public async Task<Mesa> CreateMesaService(MesaViewModel model)
        {
            if (model.NumeroMesa == null)
                throw new ArgumentException("Número da mesa não informado.");

            var numeroCadastrado = await NumeroCadastrado(model.NumeroMesa.Value);

            if (numeroCadastrado)
                throw new ArgumentException("Número já cadastrado.");

            var mesaNova = _mapper.Map<Mesa>(model);
            mesaNova.StatusMesa = false;

            return mesaNova;
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