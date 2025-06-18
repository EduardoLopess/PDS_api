using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ISaborRepository : IBaseRepository<Sabor>
    {
        Task CreateSabor(Sabor entity);
        Task<bool> MudarDisponibilidadeAsync(int entityId);
    }
}