using System.Runtime.Serialization;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class MesaRepository : IMesaRepository
    {
        private readonly DataContext _context;
        public MesaRepository(DataContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Mesa entity)
        {
            _context.Mesas.Add(entity);
            await
                _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int entityId)
        {
            var mesa = await _context.Mesas.FindAsync(entityId);
            if (mesa != null)
            {
                _context.Mesas.Remove(mesa);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<IList<Mesa>> GetAllAsync()
        {
            return await _context.Mesas
                .ToListAsync();
        }

        public async Task<Mesa?> GetByIdAsync(int entityId)
        {

            return
                await _context.Mesas
                    .SingleOrDefaultAsync(m => m.Id == entityId);

        }

        public async Task<bool> MudaStatusMesaAsync(int entityId)
        {
            var mesa = await _context.Mesas.FirstOrDefaultAsync(m => m.Id == entityId);

            if (mesa == null)
                return false;

            mesa.StatusMesa = !mesa.StatusMesa;

            await _context.SaveChangesAsync();
            return true;
        }




        public async Task UpdateAsync(Mesa entity)
        {
            var mesa = await _context.Mesas.FirstOrDefaultAsync(m => m.Id == entity.Id);
            if (mesa?.StatusMesa == true)
            {
                return;
            }

        }

        //Extra
        public async Task GetByNumeroAsync(int numeroMesa)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExisteMesaPorNumeroAsync(int numero)
        {
            return await _context.Mesas.AnyAsync(m => m.NumeroMesa == numero);
        }

        public async Task<bool> MesaOcupada(int id)
        {
            var mesa = await _context.Mesas.FirstOrDefaultAsync(m => m.Id == id);

            if (mesa == null)
                return false;

            return mesa.StatusMesa == true;
        }

    }
}