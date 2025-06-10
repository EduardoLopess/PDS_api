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

        public Task AtualizarMesa(Mesa mesa)
        {
            throw new NotImplementedException();
        }

        public async Task CriarMesa(Mesa mesa)
        {
            throw new NotImplementedException();
        }

        public Task DeletarMesa(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Mesa>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Mesa> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> MesaOcupada(int id)
        {
            throw new NotImplementedException();
        }

        public Task NumeroCadastrado(int id)
        {
            throw new NotImplementedException();
        }
    }
}